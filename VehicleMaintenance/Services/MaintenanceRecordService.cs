using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.MaintenanceRecords;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class MaintenanceRecordService(AppDbContext context, IMapper mapper) : IMaintenanceRecordService
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

        public async Task<List<MaintenanceRecordDto>> GetAllMaintenanceRecordsAsync()
        {
            var maintenanceRecords = await _context.MaintenanceRecords
                .Include(mr => mr.MaintenanceRecordComponents)
                .ToListAsync();
            return _mapper.Map<List<MaintenanceRecordDto>>(maintenanceRecords);
        }

        public async Task<MaintenanceRecordDto?> GetMaintenanceRecordByIdAsync(int id)
        {
            var maintenanceRecord = await _context.MaintenanceRecords
                .Include(mr => mr.MaintenanceRecordComponents)
                .FirstOrDefaultAsync(mr => mr.MaintenanceRecordId == id);
            return maintenanceRecord is null ? null : _mapper.Map<MaintenanceRecordDto>(maintenanceRecord);
        }

        public async Task<MaintenanceRecordDto?> UpdateMaintenanceRecordByIdAsync(int id, UpdateMaintenanceRecordDto dto)
        {
            var maintenanceRecord = await _context.MaintenanceRecords
                .Include(mr => mr.MaintenanceRecordComponents)
                .FirstOrDefaultAsync(mr => mr.MaintenanceRecordId == id);
            if (maintenanceRecord is null)
            {
                return null;
            }

            if (dto.ServiceDate.HasValue) maintenanceRecord.ServiceDate = dto.ServiceDate.Value;
            if (!string.IsNullOrWhiteSpace(dto.ServiceType)) maintenanceRecord.ServiceType = Enum.Parse<ServiceType>(dto.ServiceType, true);
            if (dto.Cost.HasValue) maintenanceRecord.Cost = dto.Cost.Value;
            if (dto.Description is not null) maintenanceRecord.Description = dto.Description;

            await _context.SaveChangesAsync();
            return _mapper.Map<MaintenanceRecordDto>(maintenanceRecord);
        }

        public async Task<bool> DeleteMaintenanceRecordByIdAsync(int id)
        {
            var maintenanceRecord = await _context.MaintenanceRecords.FirstOrDefaultAsync(mr => mr.MaintenanceRecordId == id);
            if (maintenanceRecord is null)
            {
                return false;
            }

            _context.MaintenanceRecords.Remove(maintenanceRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MaintenanceRecordDto>> GetByVehicleAsync(int vehicleId, DateTime? fromDate, DateTime? toDate, string? serviceType)
        {
            var query = _context.MaintenanceRecords
                .Where(mr => mr.VehicleId == vehicleId);

            // ServiceDate is DateTime on your entity
            if (fromDate.HasValue)
                query = query.Where(mr => mr.ServiceDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(mr => mr.ServiceDate <= toDate.Value);

            // ServiceType is an enum on your entity
            if (!string.IsNullOrWhiteSpace(serviceType))
            {
                if (Enum.TryParse<ServiceType>(serviceType, true, out var parsedServiceType))
                {
                    query = query.Where(mr => mr.ServiceType == parsedServiceType);
                }
                // else: invalid value — choose to ignore, return an error, or log as needed
            }

            var records = await query
                .OrderByDescending(mr => mr.ServiceDate)
                .ToListAsync();

            return _mapper.Map<List<MaintenanceRecordDto>>(records);
        }
    }
}