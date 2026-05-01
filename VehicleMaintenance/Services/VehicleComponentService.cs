using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class VehicleComponentService(AppDbContext context, IMapper mapper) : IVehicleComponentService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<VehicleComponentDto> CreateVehicleComponentAsync(CreateVehicleComponentDto dto)
        {
            var vehilcecomponent = _mapper.Map<VehicleComponent>(dto);

            _context.VehicleComponents.Add(vehilcecomponent);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehicleComponentDto>(vehilcecomponent);
        }

        public async Task<List<VehicleComponentDto>> GetByVehicleAsync(int vehicleId)
        {
            var components = await _context.VehicleComponents
                .Where(vc => vc.VehicleId == vehicleId)
                .ToListAsync();
            return _mapper.Map<List<VehicleComponentDto>>(components);
        }

        public async Task<List<VehicleComponentDto>> GetAllVehicleComponentsAsync()
        {
            var vehicleComponents = await _context.VehicleComponents.ToListAsync();
            return _mapper.Map<List<VehicleComponentDto>>(vehicleComponents);
        }

        public async Task<VehicleComponentDto?> GetVehicleComponentByIdAsync(int id)
        {
            var vehicleComponent = await _context.VehicleComponents.FirstOrDefaultAsync(vc => vc.VehicleComponentId == id);
            return vehicleComponent is null ? null : _mapper.Map<VehicleComponentDto>(vehicleComponent);
        }

        public async Task<VehicleComponentDto?> UpdateVehicleComponentByIdAsync(int id, UpdateVehicleComponentDto dto)
        {
            var vehicleComponent = await _context.VehicleComponents.FirstOrDefaultAsync(vc => vc.VehicleComponentId == id);
            if (vehicleComponent is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.VehicleComponentName)) vehicleComponent.VehicleComponentName = dto.VehicleComponentName;
            if (!string.IsNullOrWhiteSpace(dto.VehicleComponentBrand)) vehicleComponent.VehicleComponentBrand = dto.VehicleComponentBrand;
            if (!string.IsNullOrWhiteSpace(dto.ComponentType)) vehicleComponent.ComponentType = Enum.Parse<ComponentType>(dto.ComponentType, true);
            if (dto.InstallationDate.HasValue) vehicleComponent.InstallationDate = dto.InstallationDate.Value;
            if (dto.LastServiceDate.HasValue) vehicleComponent.LastServiceDate = dto.LastServiceDate.Value;
            if (!string.IsNullOrWhiteSpace(dto.State)) vehicleComponent.State = Enum.Parse<State>(dto.State, true);
            if (dto.Notes is not null) vehicleComponent.Notes = dto.Notes;
            if (dto.CurrentMileage.HasValue) vehicleComponent.CurrentMileage = dto.CurrentMileage.Value;
            if (dto.ExpectedLifetimeKm.HasValue) vehicleComponent.ExpectedLifetimeKm = dto.ExpectedLifetimeKm.Value;
            if (dto.ExpectedLifetimeYears.HasValue) vehicleComponent.ExpectedLifetimeYears = dto.ExpectedLifetimeYears.Value;
            if (dto.PartNumber is not null) vehicleComponent.PartNumber = dto.PartNumber;
            if (dto.WarrantyKm.HasValue) vehicleComponent.WarrantyKm = dto.WarrantyKm.Value;
            if (dto.WarrantyDate.HasValue) vehicleComponent.WarrantyDate = dto.WarrantyDate.Value;
            if (dto.NextServiceRecommendedKm.HasValue) vehicleComponent.NextServiceRecommendedKm = dto.NextServiceRecommendedKm.Value;
            if (dto.NextServiceRecommendedDate.HasValue) vehicleComponent.NextServiceRecommendedDate = dto.NextServiceRecommendedDate.Value;

            await _context.SaveChangesAsync();
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

        public async Task<List<ComponentHealthDto>> GetComponentHealthAsync(int vehicleId) // dive into this
        {
            var components = await _context.VehicleComponents
                .Where(c => c.VehicleId == vehicleId)
                .ToListAsync();

            var now = DateTime.UtcNow;

            return [.. components.Select(c =>
            {
                // KM-based health
                var remainingKm = c.ExpectedLifetimeKm - c.CurrentMileage;
                var kmPercent = c.ExpectedLifetimeKm > 0
                    ? Math.Max(0, (double)remainingKm / c.ExpectedLifetimeKm * 100)
                    : 100;

                // Year-based health — uses InstallationDate from your entity
                var yearsUsed = (now - c.InstallationDate).TotalDays / 365.25;
                var yearsPercent = c.ExpectedLifetimeYears > 0
                    ? Math.Max(0, (1 - yearsUsed / c.ExpectedLifetimeYears) * 100)
                    : 100;

                // Worst of the two determines overall status
                var worstPercent = Math.Min(kmPercent, yearsPercent);

                var status = worstPercent switch
                {
                    <= 15 => "Critical",
                    <= 30 => "Warning",
                    <= 50 => "Monitor",
                    <= 75 => "Good",
                    _ => "Excelent"
                };

                return new ComponentHealthDto
                {
                    ComponentId = c.VehicleComponentId,
                    VehicleComponentName = c.VehicleComponentName,
                    VehicleComponentBrand = c.VehicleComponentBrand,
                    ComponentType = c.ComponentType.ToString(),
                    CurrentState = c.State.ToString(),
                    InstallationDate = c.InstallationDate,
                    RemainingKm = Math.Max(0, remainingKm),
                    KmLifetimePercent = Math.Round(kmPercent, 1),
                    YearsLifetimePercent = Math.Round(yearsPercent, 1),
                    Status = status,
                };
            })];
        }

        public async Task<List<ComponentHistoryDto>> GetComponentHistoryAsync(int componentId)
        {
            var items = await _context.MaintenanceRecordComponents
                .Where(mrc => mrc.ComponentId == componentId)
                .Include(mrc => mrc.MaintenanceRecord)
                .OrderByDescending(mrc => mrc.MaintenanceRecord.ServiceDate)
                .ToListAsync();

            return [.. items.Select(mrc => new ComponentHistoryDto
            {
                MaintenanceRecordComponentId = mrc.MaintenanceRecordComponentId,
                MaintenanceRecordId = mrc.MaintenanceRecordId,
                ServiceDate = mrc.MaintenanceRecord.ServiceDate,
                ServiceName = mrc.MaintenanceRecord.ServiceName,
                ServiceType = mrc.MaintenanceRecord.ServiceType.ToString(),
                Mileage = mrc.MaintenanceRecord.Mileage,
                TechnicianName = mrc.MaintenanceRecord.TechnicianName,
                Notes = mrc.MaintenanceRecord.Notes,
                ComponentChangeType = mrc.ComponentChangeType.ToString(),
                CustomerComplaint = mrc.CustomerComplaint,
                WorkDescription = mrc.WorkDescription,
                ChangedParts = mrc.ChangedParts,
                OldState = mrc.OldState.ToString(),
                NewState = mrc.NewState.ToString(),
                ExpectedLifetimeKm = mrc.ExpectedLifetimeKm,
                ExpectedLifetimeYears = mrc.ExpectedLifetimeYears,
                LaborCost = mrc.LaborCost,
                PartsCost = mrc.PartsCost,
                OtherCost = mrc.OtherCost,
                TotalCost = mrc.TotalCost,
                CreatedAt = mrc.CreatedAt,
            })];
        }
    }
}
