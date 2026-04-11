namespace VehicleMaintenance.DTOs.Vehicles
{
    public class TimelineEventDto
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Cost { get; set; }
    }
}
