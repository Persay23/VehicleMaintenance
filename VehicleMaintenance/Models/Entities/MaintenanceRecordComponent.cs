using System;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class MaintenanceRecordComponent
    {
        public int MaintenanceRecordComponentId { get; private set; }
        public int MaintenanceRecordId { get; set; }
        public int ComponentId { get; set; }

        public ComponentChangeType ComponentChangeType { get; set; }
        public string? WorkDescription { get; set; }
        public string? ChangedParts { get; set; }
        public State OldState { get; set; } = State.Unknown;
        public State NewState{get => _newState == State.Unknown ? OldState : _newState; set => _newState = value;}
        private State _newState = State.Unknown;
        
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? LaborDays { get; set; }

        public decimal? LaborCost { get; set; }
        public decimal? PartsCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? TotalCost { get; set; }

        public string? TechnicianName { get; set; }
        public string? Vendor { get; set; }
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public MaintenanceRecord MaintenanceRecord { get; set; } = null!;
        public VehicleComponent Component { get; set; } = null!;

        
    }

}
