using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class MaintenanceRecord
    {
        public int MaintenanceRecordId { get; private set; }
        public int VehicleId { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTime ServiceDate { get; set; }
        public ServiceType ServiceType { get; set; } // enum?
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public List<MaintenanceRecordComponent> MaintenanceRecordComponents { get; set; } = [];

    }
}
