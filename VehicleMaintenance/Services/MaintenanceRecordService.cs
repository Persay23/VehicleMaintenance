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

            if (dto.Mileage.HasValue)
            {
                var vehicle = await _context.Vehicles.FindAsync(dto.VehicleId);
                if (vehicle != null && dto.Mileage.Value > vehicle.Mileage)
                    vehicle.Mileage = dto.Mileage.Value;
            }

            await _context.SaveChangesAsync();

            if (dto.PredictionId.HasValue)
            {
                var prediction = await _context.Predictions.FindAsync(dto.PredictionId.Value);
                if (prediction != null)
                {
                    prediction.Status = PredictionStatus.Completed;
                    prediction.CompletedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }

            return _mapper.Map<MaintenanceRecordDto>(maintenancerecord);
        }

        public async Task<List<MaintenanceRecordDto>> GetAllMaintenanceRecordsAsync()
        {
            var maintenanceRecords = await _context.MaintenanceRecords
                .Include(mr => mr.MaintenanceRecordComponents)
                    .ThenInclude(mrc => mrc.Component)
                .ToListAsync();
            return _mapper.Map<List<MaintenanceRecordDto>>(maintenanceRecords);
        }

        public async Task<MaintenanceRecordDto?> GetMaintenanceRecordByIdAsync(int id)
        {
            var maintenanceRecord = await _context.MaintenanceRecords
                .Include(mr => mr.MaintenanceRecordComponents)
                    .ThenInclude(mrc => mrc.Component)
                .FirstOrDefaultAsync(mr => mr.MaintenanceRecordId == id);
            return maintenanceRecord is null ? null : _mapper.Map<MaintenanceRecordDto>(maintenanceRecord);
        }

        public async Task<MaintenanceRecordDto?> UpdateMaintenanceRecordByIdAsync(int id, UpdateMaintenanceRecordDto dto)
        {
            var maintenanceRecord = await _context.MaintenanceRecords
                .Include(mr => mr.MaintenanceRecordComponents)
                    .ThenInclude(mrc => mrc.Component)
                .FirstOrDefaultAsync(mr => mr.MaintenanceRecordId == id);
            if (maintenanceRecord is null)
            {
                return null;
            }

            if (dto.ServiceName is not null) maintenanceRecord.ServiceName = dto.ServiceName;
            if (dto.ServiceDate.HasValue) maintenanceRecord.ServiceDate = dto.ServiceDate.Value;
            if (dto.StartedAt.HasValue) maintenanceRecord.StartedAt = dto.StartedAt.Value;
            if (dto.CompletedAt.HasValue) maintenanceRecord.CompletedAt = dto.CompletedAt.Value;
            if (dto.LaborDays.HasValue) maintenanceRecord.LaborDays = dto.LaborDays.Value;
            if (!string.IsNullOrWhiteSpace(dto.ServiceType)) maintenanceRecord.ServiceType = Enum.Parse<ServiceType>(dto.ServiceType, true);
            if (dto.Mileage.HasValue)
            {
                maintenanceRecord.Mileage = dto.Mileage.Value;
                var vehicle = await _context.Vehicles.FindAsync(maintenanceRecord.VehicleId);
                if (vehicle != null && dto.Mileage.Value > vehicle.Mileage)
                    vehicle.Mileage = dto.Mileage.Value;
            }
            if (dto.Cost.HasValue) maintenanceRecord.Cost = dto.Cost.Value;
            if (dto.Description is not null) maintenanceRecord.Description = dto.Description;
            if (dto.TechnicianName is not null) maintenanceRecord.TechnicianName = dto.TechnicianName;
            if (dto.VendorOrShop is not null) maintenanceRecord.VendorOrShop = dto.VendorOrShop;
            if (dto.Notes is not null) maintenanceRecord.Notes = dto.Notes;
            if (dto.InvoiceNumber is not null) maintenanceRecord.InvoiceNumber = dto.InvoiceNumber;
            if (dto.InvoiceImageUrl is not null) maintenanceRecord.InvoiceImageUrl = dto.InvoiceImageUrl;

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

            if (fromDate.HasValue)
                query = query.Where(mr => mr.ServiceDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(mr => mr.ServiceDate <= toDate.Value);

            if (!string.IsNullOrWhiteSpace(serviceType))
            {
                if (Enum.TryParse<ServiceType>(serviceType, true, out var parsedServiceType))
                {
                    query = query.Where(mr => mr.ServiceType == parsedServiceType);
                }
                else
                {
                    return [];
                }
            }

            var records = await query
                .Include(mr => mr.MaintenanceRecordComponents)
                    .ThenInclude(mrc => mrc.Component)
                .OrderByDescending(mr => mr.ServiceDate)
                .ToListAsync();

            return _mapper.Map<List<MaintenanceRecordDto>>(records);
        }
    }
}