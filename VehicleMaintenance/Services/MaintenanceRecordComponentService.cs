using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.AI;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class MaintenanceRecordComponentService(
        AppDbContext context,
        IMapper mapper,
        IAiPredictionService aiPrediction,
        ILogger<MaintenanceRecordComponentService> logger) : IMaintenanceRecordComponentService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IAiPredictionService _aiPrediction = aiPrediction;
        private readonly ILogger<MaintenanceRecordComponentService> _logger = logger;

        public async Task<List<MaintenanceRecordComponentDto>> GetAllMaintenanceRecordComponentsAsync()
        {
            var maintenanceRecordComponents = await _context.MaintenanceRecordComponents
                .Include(mrc => mrc.Component)
                .ToListAsync();
            return _mapper.Map<List<MaintenanceRecordComponentDto>>(maintenanceRecordComponents);
        }

        public async Task<MaintenanceRecordComponentDto?> GetMaintenanceRecordComponentByIdAsync(int id)
        {
            var maintenanceRecordComponent = await _context.MaintenanceRecordComponents
                .Include(mrc => mrc.Component)
                .FirstOrDefaultAsync(mrc => mrc.MaintenanceRecordComponentId == id);
            return maintenanceRecordComponent is null ? null : _mapper.Map<MaintenanceRecordComponentDto>(maintenanceRecordComponent);
        }

        public async Task<MaintenanceRecordComponentDto> CreateMaintenanceRecordComponentAsync(CreateMaintenanceRecordComponentDto dto) // dive into
        {
            var maintenanceRecordComponent = _mapper.Map<MaintenanceRecordComponent>(dto);
            _context.MaintenanceRecordComponents.Add(maintenanceRecordComponent);
            await _context.SaveChangesAsync();

            //   Auto-patch the VehicleComponent
            var component = await _context.VehicleComponents
                .FindAsync(dto.ComponentId);

            if (component != null)
            {
                // Always update state and last service date
                if (!string.IsNullOrEmpty(dto.NewState))
                    component.State = Enum.Parse<State>(dto.NewState, true);

                // Get the parent record's date
                var record = await _context.MaintenanceRecords
                    .FindAsync(dto.MaintenanceRecordId);

                if (record != null)
                    component.LastServiceDate = record.ServiceDate;

                // On replacement — record the vehicle odometer at replacement as the new install baseline
                if (Enum.Parse<ComponentChangeType>(dto.ComponentChangeType, true) == ComponentChangeType.Replaced)
                {
                    if (record != null)
                    {
                        component.InstalledAtVehicleMileage  = record.Mileage ?? component.InstalledAtVehicleMileage; // odometer at replacement = new install baseline
                        component.InstallationDate = record.ServiceDate;
                    }
                }

                // Auto-complete any Active predictions linked to this component.
                // The "Mark as completed" button on the UI is a manual fallback only;
                // logging a service record for a component is the primary completion trigger.
                var linkedPredictions = await _context.Predictions
                    .Where(p => p.VehicleComponentId == dto.ComponentId && p.Status == PredictionStatus.Active)
                    .ToListAsync();
                foreach (var pred in linkedPredictions)
                {
                    pred.Status = PredictionStatus.Completed;
                    pred.CompletedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }

            await _context.Entry(maintenanceRecordComponent).Reference(e => e.Component).LoadAsync();

            var componentId = dto.ComponentId;
            var parentRecord = await _context.MaintenanceRecords.FindAsync(dto.MaintenanceRecordId);
            var vehicleId = parentRecord?.VehicleId ?? 0;
            if (vehicleId > 0)
                _aiPrediction.TriggerBackgroundUpdate(componentId, vehicleId);
            else
                _logger.LogWarning("Background AI skipped — could not resolve vehicleId for record component {ComponentId}.", componentId);

            return _mapper.Map<MaintenanceRecordComponentDto>(maintenanceRecordComponent);
        }

        public async Task<MaintenanceRecordComponentDto?> UpdateMaintenanceRecordComponentByIdAsync(int id, UpdateMaintenanceRecordComponentDto dto)
        {
            var maintenanceRecordComponent = await _context.MaintenanceRecordComponents
                .Include(mrc => mrc.Component)
                .FirstOrDefaultAsync(mrc => mrc.MaintenanceRecordComponentId == id);
            if (maintenanceRecordComponent is null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(dto.ComponentChangeType)) maintenanceRecordComponent.ComponentChangeType = Enum.Parse<ComponentChangeType>(dto.ComponentChangeType, true);
            if (dto.WorkDescription is not null) maintenanceRecordComponent.WorkDescription = dto.WorkDescription;
            if (dto.ChangedParts is not null) maintenanceRecordComponent.ChangedParts = dto.ChangedParts;
            if (!string.IsNullOrEmpty(dto.OldState)) maintenanceRecordComponent.OldState = Enum.Parse<State>(dto.OldState, true);
            if (!string.IsNullOrEmpty(dto.NewState)) maintenanceRecordComponent.NewState = Enum.Parse<State>(dto.NewState, true);
            if (dto.StartedAt.HasValue) maintenanceRecordComponent.StartedAt = dto.StartedAt.Value;
            if (dto.CompletedAt.HasValue) maintenanceRecordComponent.CompletedAt = dto.CompletedAt.Value;
            if (dto.LaborDays.HasValue) maintenanceRecordComponent.LaborDays = dto.LaborDays.Value;
            if (dto.LaborCost.HasValue) maintenanceRecordComponent.LaborCost = dto.LaborCost.Value;
            if (dto.PartsCost.HasValue) maintenanceRecordComponent.PartsCost = dto.PartsCost.Value;
            if (dto.OtherCost.HasValue) maintenanceRecordComponent.OtherCost = dto.OtherCost.Value;
            if (dto.TotalCost.HasValue) maintenanceRecordComponent.TotalCost = dto.TotalCost.Value;
            if (dto.CustomerComplaint is not null) maintenanceRecordComponent.CustomerComplaint = dto.CustomerComplaint;
            if (dto.ExpectedLifetimeKm.HasValue) maintenanceRecordComponent.ExpectedLifetimeKm = dto.ExpectedLifetimeKm.Value;
            if (dto.ExpectedLifetimeYears.HasValue) maintenanceRecordComponent.ExpectedLifetimeYears = dto.ExpectedLifetimeYears.Value;
            maintenanceRecordComponent.UpdatedAt = DateTime.UtcNow;

            if (maintenanceRecordComponent.ComponentChangeType == ComponentChangeType.Replaced)
            {
                var component = await _context.VehicleComponents.FindAsync(maintenanceRecordComponent.ComponentId);
                var record = await _context.MaintenanceRecords.FindAsync(maintenanceRecordComponent.MaintenanceRecordId);
                if (component != null && record != null)
                    component.InstallationDate = record.ServiceDate;
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<MaintenanceRecordComponentDto>(maintenanceRecordComponent);
        }

        public async Task<bool> DeleteMaintenanceRecordComponentByIdAsync(int id)
        {
            var maintenanceRecordComponent = await _context.MaintenanceRecordComponents
                .FirstOrDefaultAsync(mrc => mrc.MaintenanceRecordComponentId == id);

            if (maintenanceRecordComponent is null)
            {
                return false;
            }

            _context.MaintenanceRecordComponents.Remove(maintenanceRecordComponent);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
