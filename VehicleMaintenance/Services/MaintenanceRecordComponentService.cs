using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class MaintenanceRecordComponentService(AppDbContext context, IMapper mapper) : IMaintenanceRecordComponentService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<MaintenanceRecordComponentDto> CreateMaintenanceRecordComponentAsync(CreateMaintenanceRecordComponentDto dto)
        {
            var maintenanceRecordComponent = _mapper.Map<MaintenanceRecordComponent>(dto);

            _context.MaintenanceRecordComponents.Add(maintenanceRecordComponent);
            await _context.SaveChangesAsync();

            return _mapper.Map<MaintenanceRecordComponentDto>(maintenanceRecordComponent);
        }

        public async Task<List<MaintenanceRecordComponentDto>> GetAllMaintenanceRecordComponentsAsync()
        {
            var maintenanceRecordComponents = await _context.MaintenanceRecordComponents.ToListAsync();
            return _mapper.Map<List<MaintenanceRecordComponentDto>>(maintenanceRecordComponents);
        }

        public async Task<MaintenanceRecordComponentDto?> GetMaintenanceRecordComponentByIdAsync(int id)
        {
            var maintenanceRecordComponent = await _context.MaintenanceRecordComponents
                .FirstOrDefaultAsync(mrc => mrc.MaintenanceRecordComponentId == id);
            return maintenanceRecordComponent is null ? null : _mapper.Map<MaintenanceRecordComponentDto>(maintenanceRecordComponent);
        }

        public async Task<MaintenanceRecordComponentDto?> UpdateMaintenanceRecordComponentByIdAsync(int id, UpdateMaintenanceRecordComponentDto dto)
        {
            var maintenanceRecordComponent = await _context.MaintenanceRecordComponents
                .FirstOrDefaultAsync(mrc => mrc.MaintenanceRecordComponentId == id);
            if (maintenanceRecordComponent is null)
            {
                return null;
            }

            if (dto.ChangeType.HasValue) maintenanceRecordComponent.ChangeType = dto.ChangeType.Value;
            if (dto.WorkDescription is not null) maintenanceRecordComponent.WorkDescription = dto.WorkDescription;
            if (dto.ChangedParts is not null) maintenanceRecordComponent.ChangedParts = dto.ChangedParts;
            if (dto.OldState.HasValue) maintenanceRecordComponent.OldState = dto.OldState.Value;
            if (dto.NewState.HasValue) maintenanceRecordComponent.NewState = dto.NewState.Value;
            if (dto.StartedAt.HasValue) maintenanceRecordComponent.StartedAt = dto.StartedAt.Value;
            if (dto.CompletedAt.HasValue) maintenanceRecordComponent.CompletedAt = dto.CompletedAt.Value;
            if (dto.LaborDays.HasValue) maintenanceRecordComponent.LaborDays = dto.LaborDays.Value;
            if (dto.LaborCost.HasValue) maintenanceRecordComponent.LaborCost = dto.LaborCost.Value;
            if (dto.PartsCost.HasValue) maintenanceRecordComponent.PartsCost = dto.PartsCost.Value;
            if (dto.OtherCost.HasValue) maintenanceRecordComponent.OtherCost = dto.OtherCost.Value;
            if (dto.TotalCost.HasValue) maintenanceRecordComponent.TotalCost = dto.TotalCost.Value;
            if (dto.TechnicianName is not null) maintenanceRecordComponent.TechnicianName = dto.TechnicianName;
            if (dto.Vendor is not null) maintenanceRecordComponent.VendorOrShop = dto.Vendor;
            if (dto.Notes is not null) maintenanceRecordComponent.Notes = dto.Notes;

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
