using System.ComponentModel.DataAnnotations;

namespace VehicleMaintenance.DTOs.AI
{
    public class DiagnoseRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(1000)]
        public string Symptom { get; set; } = null!;
    }
}
