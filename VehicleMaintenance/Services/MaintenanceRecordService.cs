using VehicleMaintenance.Data;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.DTOs.MaintenanceRecords;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Services
{
    public class MaintenanceRecordService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<MaintenanceRecordDto> CreateMaintenanceRecordAsync(CreateMaintenanceRecordDto dto)
        {
            var maintenancerecord = new MaintenanceRecord
            {
                VehicleId = dto.VehicleId,
                ServiceDate = dto.ServiceDate,
                ServiceType = dto.ServiceType,
                Cost = dto.Cost,
                Description = dto.Description
            };

            _context.MaintenanceRecords.Add(maintenancerecord);
            await _context.SaveChangesAsync();

            return new MaintenanceRecordDto
            {
                MaintenanceRecordId = maintenancerecord.MaintenanceRecordId,
                VehicleId = maintenancerecord.VehicleId,
                ServiceDate = dto.ServiceDate,
                ServiceType = dto.ServiceType,
                Cost = dto.Cost,
                Description = dto.Description
            };
        }

        public async Task<List<MaintenanceRecordDto>> GetMaintenanceRecordsByVehicleIdAsync(int vehicleId)
        {
            return await _context.MaintenanceRecords
                .Where(mr => mr.VehicleId == vehicleId)
                .Select(mr => new MaintenanceRecordDto
                {
                    MaintenanceRecordId = mr.MaintenanceRecordId,
                    VehicleId = mr.VehicleId,
                    ServiceDate = mr.ServiceDate,
                    ServiceType = mr.ServiceType,
                    Cost = mr.Cost,
                    Description = mr.Description
                })
                .ToListAsync();
        }
    }
}