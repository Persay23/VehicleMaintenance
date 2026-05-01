namespace VehicleMaintenance.DTOs.Vehicles
{
    public class MonthlyCostDto
    {
        public DateTime Month { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal FuelCost { get; set; }
    }
}
