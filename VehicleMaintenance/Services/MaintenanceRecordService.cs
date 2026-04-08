using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.DTOs.MaintenanceRecords;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Services
{
    public class MaintenanceRecordService(AppDbContext context, IMapper mapper)
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<MaintenanceRecordDto> CreateMaintenanceRecordAsync(CreateMaintenanceRecordDto dto)
        {
            var maintenancerecord = _mapper.Map<MaintenanceRecord>(dto);

            _context.MaintenanceRecords.Add(maintenancerecord);
            await _context.SaveChangesAsync();

            return _mapper.Map<MaintenanceRecordDto>(maintenancerecord);
        }

        public async Task<List<MaintenanceRecordDto>> GetMaintenanceRecordsByVehicleIdAsync(int vehicleId)
        {
            var maintenanceRecords = await _context.MaintenanceRecords
                .Include(mr => mr.MaintenanceRecordComponents)
                .Where(mr => mr.VehicleId == vehicleId)
                .ToListAsync();

            return _mapper.Map<List<MaintenanceRecordDto>>(maintenanceRecords);
        }
    }
}