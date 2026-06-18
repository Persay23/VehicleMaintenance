using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Models;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.AI;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class VehicleComponentService(
        AppDbContext context,
        IMapper mapper,
        IAiPredictionService aiPrediction,
        ILogger<VehicleComponentService> logger) : IVehicleComponentService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IAiPredictionService _aiPrediction = aiPrediction;
        private readonly ILogger<VehicleComponentService> _logger = logger;
        public async Task<List<VehicleComponentDto>> GetVehicleComponentByVehicleAsync(int vehicleId)
        {
            var components = await _context.VehicleComponents
                .Where(vc => vc.VehicleId == vehicleId)
                .Include(vc => vc.Vehicle)
                .ToListAsync();
            return _mapper.Map<List<VehicleComponentDto>>(components);
        }

        public async Task<List<VehicleComponentDto>> GetAllVehicleComponentsAsync()
        {
            var vehicleComponents = await _context.VehicleComponents
                .Include(vc => vc.Vehicle)
                .ToListAsync();
            return _mapper.Map<List<VehicleComponentDto>>(vehicleComponents);
        }

        public async Task<VehicleComponentDto?> GetVehicleComponentByIdAsync(int id)
        {
            var vehicleComponent = await _context.VehicleComponents
                .Include(vc => vc.Vehicle)
                .FirstOrDefaultAsync(vc => vc.VehicleComponentId == id);
            return vehicleComponent is null ? null : _mapper.Map<VehicleComponentDto>(vehicleComponent);
        }
        public async Task<List<ComponentHistoryDto>> GetComponentHistoryAsync(int componentId)
        {
            var items = await _context.MaintenanceRecordComponents
                .Where(mrc => mrc.ComponentId == componentId)
                .Include(mrc => mrc.MaintenanceRecord)
                .OrderByDescending(mrc => mrc.MaintenanceRecord.ServiceDate)
                .ToListAsync();

            return _mapper.Map<List<ComponentHistoryDto>>(items);
        }
        public async Task<List<ComponentHealthDto>> GetComponentHealthAsync(int vehicleId)
        {
            var components = await _context.VehicleComponents
                .Where(c => c.VehicleId == vehicleId)
                .Include(c => c.Vehicle)
                .ToListAsync();

            return [.. components.Select(c => CalculateHealth(c, c.Vehicle.Mileage))];
        }

        public async Task<VehicleComponentDto> CreateVehicleComponentAsync(CreateVehicleComponentDto dto)
        {
            var vehilcecomponent = _mapper.Map<VehicleComponent>(dto);

            var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId);
            if (vehicle != null)
            {
                var health = CalculateHealth(vehilcecomponent, vehicle.Mileage);
                vehilcecomponent.State = Enum.Parse<State>(health.Status, true);
            }

            _context.VehicleComponents.Add(vehilcecomponent);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehicleComponentDto>(vehilcecomponent);
        }

        public async Task<VehicleComponentDto?> UpdateVehicleComponentByIdAsync(int id, UpdateVehicleComponentDto dto)
        {
            var vehicleComponent = await _context.VehicleComponents
                .Include(vc => vc.Vehicle)
                .FirstOrDefaultAsync(vc => vc.VehicleComponentId == id);
            if (vehicleComponent is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.VehicleComponentName)) vehicleComponent.VehicleComponentName = dto.VehicleComponentName;
            if (!string.IsNullOrWhiteSpace(dto.VehicleComponentBrand)) vehicleComponent.VehicleComponentBrand = dto.VehicleComponentBrand;
            if (!string.IsNullOrWhiteSpace(dto.ComponentType)) vehicleComponent.ComponentType = Enum.Parse<ComponentType>(dto.ComponentType, true);
            if (dto.InstallationDate.HasValue) vehicleComponent.InstallationDate = dto.InstallationDate.Value;
            if (dto.LastServiceDate.HasValue) vehicleComponent.LastServiceDate = dto.LastServiceDate.Value;
            if (dto.Notes is not null) vehicleComponent.Notes = dto.Notes;
            if (dto.InstalledAtVehicleMileage.HasValue) vehicleComponent.InstalledAtVehicleMileage = dto.InstalledAtVehicleMileage.Value;
            if (dto.ExpectedLifetimeKm.HasValue) vehicleComponent.ExpectedLifetimeKm = dto.ExpectedLifetimeKm.Value;
            if (dto.ExpectedLifetimeYears.HasValue) vehicleComponent.ExpectedLifetimeYears = dto.ExpectedLifetimeYears.Value;
            if (dto.PartNumber is not null) vehicleComponent.PartNumber = dto.PartNumber;
            if (dto.WarrantyKm.HasValue) vehicleComponent.WarrantyKm = dto.WarrantyKm.Value;
            if (dto.WarrantyDate.HasValue) vehicleComponent.WarrantyDate = dto.WarrantyDate.Value;
            if (dto.NextServiceRecommendedKm.HasValue) vehicleComponent.NextServiceRecommendedKm = dto.NextServiceRecommendedKm.Value;
            if (dto.NextServiceRecommendedDate.HasValue) vehicleComponent.NextServiceRecommendedDate = dto.NextServiceRecommendedDate.Value;

            // Always derive state from computed health so DB, AI prompt, and UI stay in sync
            if (vehicleComponent.Vehicle != null)
            {
                var health = CalculateHealth(vehicleComponent, vehicleComponent.Vehicle.Mileage);
                vehicleComponent.State = Enum.Parse<State>(health.Status, true);
            }

            await _context.SaveChangesAsync();

            _aiPrediction.TriggerBackgroundUpdate(vehicleComponent.VehicleComponentId, vehicleComponent.VehicleId);

            return _mapper.Map<VehicleComponentDto>(vehicleComponent);
        }

        public async Task<bool> DeleteVehicleComponentByIdAsync(int id)  // dive into this
        {
            var component = await _context.VehicleComponents
                .FirstOrDefaultAsync(c => c.VehicleComponentId == id);

            if (component is null) return false;

            // Delete linked MaintenanceRecordComponents first
            // because the FK is NoAction — SQL won't cascade automatically
            var linkedRecords = await _context.MaintenanceRecordComponents
                .Where(mrc => mrc.ComponentId == id)
                .ToListAsync();

            if (linkedRecords.Count != 0)
                _context.MaintenanceRecordComponents.RemoveRange(linkedRecords);

            // Also clear any predictions linked to this component
            var linkedPredictions = await _context.Predictions
                .Where(p => p.VehicleComponentId == id)
                .ToListAsync();

            if (linkedPredictions.Count != 0)
                _context.Predictions.RemoveRange(linkedPredictions);

            _context.VehicleComponents.Remove(component);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ComponentHealthDto CalculateHealth(VehicleComponent c, int vehicleCurrentMileage)
        {
            // Unknown: install mileage reference not set (vehicle already has mileage → km calc unreliable)
            // Unknown: no lifetime limits configured at all
            bool kmRefMissing = c.InstalledAtVehicleMileage == 0 && vehicleCurrentMileage > 0;
            bool noLimits     = c.ExpectedLifetimeKm == 0 && c.ExpectedLifetimeYears == 0;

            if (kmRefMissing || noLimits)
            {
                return new ComponentHealthDto
                {
                    ComponentId          = c.VehicleComponentId,
                    VehicleComponentName = c.VehicleComponentName,
                    VehicleComponentBrand = c.VehicleComponentBrand,
                    ComponentType        = c.ComponentType.ToString(),
                    CurrentState         = "Unknown",
                    Status               = "Unknown",
                    InstallationDate     = c.InstallationDate,
                    RemainingKm          = 0,
                    KmLifetimePercent    = 0,
                    YearsLifetimePercent = 0,
                    AiEstimatedNextServiceDate = c.AiEstimatedNextServiceDate,
                    AiGeneratedAt        = c.AiGeneratedAt,
                };
            }

            var health = ComponentHealthCalculator.Compute(c, vehicleCurrentMileage);

            return new ComponentHealthDto
            {
                ComponentId           = c.VehicleComponentId,
                VehicleComponentName  = c.VehicleComponentName,
                VehicleComponentBrand = c.VehicleComponentBrand,
                ComponentType         = c.ComponentType.ToString(),
                CurrentState          = health.State,
                InstallationDate      = c.InstallationDate,
                RemainingKm           = health.RemainingKm,
                KmLifetimePercent     = health.KmRemainingPercent,
                YearsLifetimePercent  = health.YearsRemainingPercent,
                Status                = health.State,
                AiEstimatedNextServiceDate = c.AiEstimatedNextServiceDate,
                AiGeneratedAt         = c.AiGeneratedAt,
            };
        }
    }
}
