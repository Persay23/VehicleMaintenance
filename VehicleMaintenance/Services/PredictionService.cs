using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Prediction;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class PredictionService(AppDbContext context, IMapper mapper) : IPredictionService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<PredictionDto[]> GetAllPredictionsAsync()
        {
            var predictions = await _context.Predictions
                .AsNoTracking()
                .Include(p => p.VehicleComponent)
                .ToArrayAsync();
            return _mapper.Map<PredictionDto[]>(predictions);
        }

        public async Task<PredictionDto?> GetPredictionByIdAsync(int id)
        {
            var prediction = await _context.Predictions
                .AsNoTracking()
                .Include(p => p.VehicleComponent)
                .FirstOrDefaultAsync(p => p.PredictionId == id);

            return prediction is null ? null : _mapper.Map<PredictionDto>(prediction);
        }

        public async Task<PredictionDto[]> GetPredictionsByVehicleAsync(int vehicleId)
        {
            var predictions = await _context.Predictions
                .AsNoTracking()
                .Include(p => p.VehicleComponent)
                .Where(p => p.VehicleId == vehicleId)
                .OrderBy(p => p.Urgency)
                .ThenBy(p => p.SuggestedByDate)
                .ToArrayAsync();

            return _mapper.Map<PredictionDto[]>(predictions);
        }

        public async Task<PredictionDto?> UpdatePredictionByIdAsync(int id, UpdatePredictionDto dto)
        {
            var prediction = await _context.Predictions
                .Include(p => p.VehicleComponent)
                .FirstOrDefaultAsync(p => p.PredictionId == id);

            if (prediction is null) return null;

            if (!string.IsNullOrWhiteSpace(dto.Status))
                prediction.Status = Enum.Parse<PredictionStatus>(dto.Status, true);
            if (dto.CompletedAt.HasValue)
                prediction.CompletedAt = dto.CompletedAt.Value;
            if (dto.IgnoredAt.HasValue)
                prediction.IgnoredAt = dto.IgnoredAt.Value;

            await _context.SaveChangesAsync();
            return _mapper.Map<PredictionDto>(prediction);
        }

        public async Task<bool> DeletePredictionByIdAsync(int id)
        {
            var prediction = await _context.Predictions
                .FirstOrDefaultAsync(p => p.PredictionId == id);

            if (prediction is null) return false;

            _context.Predictions.Remove(prediction);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
