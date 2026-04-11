using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Prediction;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services // ???????? wtf
{
    public class PredictionService(AppDbContext context) : IPredictionService
    {
        private readonly AppDbContext _context = context;

        public async Task<PredictionDto> CreatePredictionAsync(CreatePredictionDto dto)
        {
            var prediction = new Prediction
            {
                VehicleId = dto.VehicleId,
                ComponentType = Enum.Parse<ComponentType>(dto.ComponentType, true),
                PredictedServiceDate = dto.PredictedServiceDate,
                ConfidenceScore = dto.ConfidentScore / 100.0
            };

            _context.Predictions.Add(prediction);
            await _context.SaveChangesAsync();

            return new PredictionDto
            {
                PredictionId = prediction.PredictionId,
                VehicleId = prediction.VehicleId,
                Name = dto.Name,
                ComponentType = prediction.ComponentType.ToString(),
                PredictedServiceDate = prediction.PredictedServiceDate,
                ConfidentScore = dto.ConfidentScore
            };
        }

        public async Task<List<PredictionDto>> GetAllPredictionsAsync()
        {
            return await _context.Predictions
                .Select(p => new PredictionDto
                {
                    PredictionId = p.PredictionId,
                    VehicleId = p.VehicleId,
                    Name = string.Empty,
                    ComponentType = p.ComponentType.ToString(),
                    PredictedServiceDate = p.PredictedServiceDate,
                ConfidentScore = (int)(p.ConfidenceScore * 100)
                })
                .ToListAsync();
        }

        public async Task<PredictionDto?> GetPredictionByIdAsync(int id)
        {
            var prediction = await _context.Predictions.FirstOrDefaultAsync(p => p.PredictionId == id);
            if (prediction is null)
            {
                return null;
            }

            return new PredictionDto
            {
                PredictionId = prediction.PredictionId,
                VehicleId = prediction.VehicleId,
                Name = string.Empty,
                ComponentType = prediction.ComponentType.ToString(),
                PredictedServiceDate = prediction.PredictedServiceDate,
                ConfidentScore = (int)(prediction.ConfidenceScore * 100)
            };
        }

        public async Task<PredictionDto?> UpdatePredictionByIdAsync(int id, UpdatePredictionDto dto)
        {
            var prediction = await _context.Predictions.FirstOrDefaultAsync(p => p.PredictionId == id);
            if (prediction is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.ComponentType)) prediction.ComponentType = Enum.Parse<ComponentType>(dto.ComponentType, true);
            if (dto.PredictedServiceDate.HasValue) prediction.PredictedServiceDate = dto.PredictedServiceDate.Value;
            if (dto.ConfidentScore.HasValue) prediction.ConfidenceScore = dto.ConfidentScore.Value / 100.0;

            await _context.SaveChangesAsync();

            return new PredictionDto
            {
                PredictionId = prediction.PredictionId,
                VehicleId = prediction.VehicleId,
                Name = string.Empty,
                ComponentType = prediction.ComponentType.ToString(),
                PredictedServiceDate = prediction.PredictedServiceDate,
                ConfidentScore = (int)(prediction.ConfidenceScore * 100)
            };
        }

        public async Task<bool> DeletePredictionByIdAsync(int id)
        {
            var prediction = await _context.Predictions.FirstOrDefaultAsync(p => p.PredictionId == id);
            if (prediction is null)
            {
                return false;
            }

            _context.Predictions.Remove(prediction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
