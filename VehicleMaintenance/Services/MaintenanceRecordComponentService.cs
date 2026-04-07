using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Services
{
    public class MaintenanceRecordComponentService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<MaintenanceRecordComponentDto> CreateMaintenanceRecordComponentAsync(CreateMaintenanceRecordComponentDto dto)
        {
            var maintenanceRecordComponent = new MaintenanceRecordComponent
            {
                MaintenanceRecordId = dto.MaintenanceRecordId,
                ComponentId = dto.ComponentId,
                ChangeType = dto.ChangeType,
                WorkDescription = dto.WorkDescription,
                ChangedParts = dto.ChangedParts,
                OldState = dto.OldState,
                NewState = dto.NewState,
                StartedAt = dto.StartedAt,
                CompletedAt = dto.CompletedAt,
                LaborMinutes = dto.LaborMinutes,
                LaborCost = dto.LaborCost,
                PartsCost = dto.PartsCost,
                OtherCost = dto.OtherCost,
                TotalCost = dto.TotalCost,
                TechnicianName = dto.TechnicianName,
                VendorOrShop = dto.VendorOrShop,
                Notes = dto.Notes
            };

            _context.MaintenanceRecordComponents.Add(maintenanceRecordComponent);
            await _context.SaveChangesAsync();

            return new MaintenanceRecordComponentDto
            {
                MaintenanceRecordComponentId = maintenanceRecordComponent.MaintenanceRecordComponentId,
                MaintenanceRecordId = maintenanceRecordComponent.MaintenanceRecordId,
                ComponentId = maintenanceRecordComponent.ComponentId,
                ChangeType = maintenanceRecordComponent.ChangeType,
                WorkDescription = maintenanceRecordComponent.WorkDescription,
                ChangedParts = maintenanceRecordComponent.ChangedParts,
                OldState = maintenanceRecordComponent.OldState,
                NewState = maintenanceRecordComponent.NewState,
                StartedAt = maintenanceRecordComponent.StartedAt,
                CompletedAt = maintenanceRecordComponent.CompletedAt,
                LaborMinutes = maintenanceRecordComponent.LaborMinutes,
                LaborCost = maintenanceRecordComponent.LaborCost,
                PartsCost = maintenanceRecordComponent.PartsCost,
                OtherCost = maintenanceRecordComponent.OtherCost,
                TotalCost = maintenanceRecordComponent.TotalCost,
                TechnicianName = maintenanceRecordComponent.TechnicianName,
                VendorOrShop = maintenanceRecordComponent.VendorOrShop,
                Notes = maintenanceRecordComponent.Notes,
                CreatedAt = maintenanceRecordComponent.CreatedAt,
                UpdatedAt = maintenanceRecordComponent.UpdatedAt
            };
        }

        public async Task<List<MaintenanceRecordComponentDto>> GetMaintenanceRecordComponentsByMaintenanceRecordIdAsync(int maintenanceRecordId)
        {
            return await _context.MaintenanceRecordComponents
                .Where(mrc => mrc.MaintenanceRecordId == maintenanceRecordId)
                .Select(mrc => new MaintenanceRecordComponentDto
                {
                    MaintenanceRecordComponentId = mrc.MaintenanceRecordComponentId,
                    MaintenanceRecordId = mrc.MaintenanceRecordId,
                    ComponentId = mrc.ComponentId,
                    ChangeType = mrc.ChangeType,
                    WorkDescription = mrc.WorkDescription,
                    ChangedParts = mrc.ChangedParts,
                    OldState = mrc.OldState,
                    NewState = mrc.NewState,
                    StartedAt = mrc.StartedAt,
                    CompletedAt = mrc.CompletedAt,
                    LaborMinutes = mrc.LaborMinutes,
                    LaborCost = mrc.LaborCost,
                    PartsCost = mrc.PartsCost,
                    OtherCost = mrc.OtherCost,
                    TotalCost = mrc.TotalCost,
                    TechnicianName = mrc.TechnicianName,
                    VendorOrShop = mrc.VendorOrShop,
                    Notes = mrc.Notes,
                    CreatedAt = mrc.CreatedAt,
                    UpdatedAt = mrc.UpdatedAt
                })
                .ToListAsync();
        }
    }
}
