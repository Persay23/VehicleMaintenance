namespace VehicleMaintenance.DTOs.VehicleComponents
{
    public class ComponentHistoryDto
    {
        public int MaintenanceRecordComponentId { get; set; }
        public int MaintenanceRecordId { get; set; }

        // From the parent MaintenanceRecord
        public DateTime ServiceDate { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ServiceType { get; set; } = null!;

        public int? Mileage { get; set; }
        public string? TechnicianName { get; set; }
        public string? Notes { get; set; }

        // From MaintenanceRecordComponent
        public string ComponentChangeType { get; set; } = null!;
        public string? CustomerComplaint { get; set; }
        public string? WorkDescription { get; set; }
        public string? ChangedParts { get; set; }
        public string? OldState { get; set; }
        public string? NewState { get; set; }
        public int? ExpectedLifetimeKm { get; set; }
        public int? ExpectedLifetimeYears { get; set; }
        public decimal? LaborCost { get; set; }
        public decimal? PartsCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}