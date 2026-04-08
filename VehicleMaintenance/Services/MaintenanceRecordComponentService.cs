using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Services
{
    public class MaintenanceRecordComponentService(AppDbContext context, IMapper mapper)
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

        public async Task<List<MaintenanceRecordComponentDto>> GetMaintenanceRecordComponentsByMaintenanceRecordIdAsync(int maintenanceRecordId)
        {
            var maintenanceRecordComponents = await _context.MaintenanceRecordComponents
                .Where(mrc => mrc.MaintenanceRecordId == maintenanceRecordId)
                .ToListAsync();

            return _mapper.Map<List<MaintenanceRecordComponentDto>>(maintenanceRecordComponents);
        }
    }
}
