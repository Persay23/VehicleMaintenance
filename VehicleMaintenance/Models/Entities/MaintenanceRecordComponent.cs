using System;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Models.Entities
{
    public class MaintenanceRecordComponent
    {
        public int MaintenanceRecordComponentId { get; private set; }
        public int MaintenanceRecordId { get; set; }
        public int ComponentId { get; set; }

        public ComponentChangeType ComponentChangeType { get; set; } // replaced, inspected, lubricated // later to this must be linked actions like reset mileage if component was replaced, etc.
        public string? WorkDescription { get; set; }
        public string? ChangedParts { get; set; }
        public State OldState { get; set; } = State.Unknown;
        public State NewState{get => _newState == State.Unknown ? OldState : _newState; set => _newState = value;} // buisness logic, needs to be implemented in service layer, if new state is unknown, it should be set to old state, otherwise it should be set to new state
        private State _newState = State.Unknown;
        
        public DateTime? StartedAt { get; set; } // those field are already present in maintenance record, probably should be removed
        public DateTime? CompletedAt { get; set; } // same
        public int? LaborDays { get; set; } // same

        public decimal? LaborCost { get; set; }
        public decimal? PartsCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? TotalCost { get; set; }

        public string? CustomerComplaint { get; set; }
        public int? ExpectedLifetimeKm { get; set; } // does the customer know how many km the component should last? maybe this field should be derived by AI, because mechanics will not put this by themselves, or maybe prefill it with regular lifetime of the component based on the type and brand, and then users can adjust if needed
        public int? ExpectedLifetimeYears { get; set; } // same as above,

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public MaintenanceRecord MaintenanceRecord { get; set; } = null!;
        public VehicleComponent Component { get; set; } = null!;

        
    }

}
