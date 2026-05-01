namespace VehicleMaintenance.DTOs.Vehicles
{
    public class TimelineEventDto
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Cost { get; set; }
        //public int VehicleId { get; set; }
        //public string VehicleName { get; set; }  // "BMW 3 Series 320d"
        //public int? RelatedId { get; set; }      // MaintenanceRecordId or LiquidEntryId
    }
}
