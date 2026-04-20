using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Prediction;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class PredictionService(AppDbContext context, IMapper mapper) : IPredictionService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<PredictionDto> CreatePredictionAsync(CreatePredictionDto dto)
        {
            var prediction = _mapper.Map<Prediction>(dto);
            // ConfidenceScore comes in as 0–100, store as 0.0–1.0
            prediction.ConfidenceScore = dto.ConfidenceScore / 100.0;

            _context.Predictions.Add(prediction);
            await _context.SaveChangesAsync();

            return _mapper.Map<PredictionDto>(prediction);
        }

        public async Task<List<PredictionDto>> GetAllPredictionsAsync()
        {
            var predictions = await _context.Predictions.ToListAsync();
            return _mapper.Map<List<PredictionDto>>(predictions);
        }

        public async Task<PredictionDto?> GetPredictionByIdAsync(int id)
        {
            var prediction = await _context.Predictions
                .FirstOrDefaultAsync(p => p.PredictionId == id);

            return prediction is null ? null : _mapper.Map<PredictionDto>(prediction);
        }

        public async Task<List<PredictionDto>> GetPredictionsByVehicleAsync(int vehicleId)
        {
            var predictions = await _context.Predictions
                .Where(p => p.VehicleId == vehicleId)
                .OrderBy(p => p.PredictedServiceDate)
                .ToListAsync();

            return _mapper.Map<List<PredictionDto>>(predictions);
        }

        public async Task<PredictionDto?> UpdatePredictionByIdAsync(int id, UpdatePredictionDto dto)
        {
            var prediction = await _context.Predictions
                .FirstOrDefaultAsync(p => p.PredictionId == id);

            if (prediction is null) return null;

            if (!string.IsNullOrWhiteSpace(dto.ComponentType))
                prediction.ComponentType = Enum.Parse<ComponentType>(dto.ComponentType, true);
            if (!string.IsNullOrWhiteSpace(dto.Name))
                prediction.Name = dto.Name;
            if (dto.PredictedServiceDate.HasValue)
                prediction.PredictedServiceDate = dto.PredictedServiceDate.Value;
            if (!string.IsNullOrWhiteSpace(dto.Status))
                prediction.Status = Enum.Parse<PredictionStatus>(dto.Status, true);
            if (dto.CompletedAt.HasValue)
                prediction.CompletedAt = dto.CompletedAt.Value;
            if (dto.ConfidenceScore.HasValue)
                prediction.ConfidenceScore = dto.ConfidenceScore.Value / 100.0;

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