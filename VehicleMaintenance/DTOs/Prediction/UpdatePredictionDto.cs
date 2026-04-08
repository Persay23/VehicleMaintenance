using System.ComponentModel.DataAnnotations;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.DTOs.Prediction
{
    public class UpdatePredictionDto
    {
        public string? Name { get; set; }
        public ComponentType? ComponentType { get; set; }
        public DateTime? PredictedServiceDate { get; set; }

        [Range(0, 100)]
        public int? ConfidentScore { get; set; }
    }
}
