using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Data;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Data;

public class DataSeeder(AppDbContext context, UserManager<User> userManager)
{
    private readonly AppDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;

    public async Task SeedAsync()
    {
        if (_userManager.Users.Any()) return;

        // ── USER ──────────────────────────────────────────────────────────────
        var user = new User
        {
            UserName = "jan@autocare.pl",
            Email = "jan@autocare.pl",
            Name = "Jan Kowalski",
            Age = 28,
            Gender = Gender.Male,
            DrivingExperience = 2018,
        };
        await _userManager.CreateAsync(user, "Test1234!");

        // ── VEHICLES ──────────────────────────────────────────────────────────
        var bmw = new Vehicle
        {
            UserId = user.Id,
            Brand = "BMW",
            Model = "3 Series 320d",
            YearOfProduction = 2019,
            VehicleType = VehicleType.Sedan,
            TransmissionType = TransmissionType.Automatic,
            EngineType = EngineType.Diesel,         // was InternalCombustion
            FuelType = FuelType.Diesel,
            Mileage = 187320,
        };

        var audi = new Vehicle
        {
            UserId = user.Id,
            Brand = "Audi",
            Model = "A4 2.0 TDI",
            YearOfProduction = 2021,
            VehicleType = VehicleType.Sedan,
            TransmissionType = TransmissionType.Manual,
            EngineType = EngineType.Diesel,         // was InternalCombustion
            FuelType = FuelType.Diesel,
            Mileage = 64500,
        };

        var honda = new Vehicle
        {
            UserId = user.Id,
            Brand = "Honda",
            Model = "Jazz 1.2",
            YearOfProduction = 2015,
            VehicleType = VehicleType.Hatchback,
            TransmissionType = TransmissionType.Automatic,
            EngineType = EngineType.Petrol,         // was InternalCombustion
            FuelType = FuelType.Petrol95,           // was Petrol
            Mileage = 297856,
        };

        _context.Vehicles.AddRange(bmw, audi, honda);
        await _context.SaveChangesAsync();

        // ── BMW COMPONENTS ────────────────────────────────────────────────────
        var bmwBrakePads = new VehicleComponent
        {
            VehicleId = bmw.VehicleId,
            ComponentType = ComponentType.Brakes,
            VehicleComponentName = "Front Axle",
            VehicleComponentBrand = "Brembo",
            InstallationDate = new DateTime(2023, 1, 15),
            State = State.Critical,
            CurrentMileage = 145200,
            ExpectedLifetimeKm = 50000,
            ExpectedLifetimeYears = 5,
            Notes = "Squeaking noise when braking",
        };

        var bmwTimingBelt = new VehicleComponent
        {
            VehicleId = bmw.VehicleId,
            ComponentType = ComponentType.Engine,
            VehicleComponentBrand = "Gates",
            InstallationDate = new DateTime(2021, 3, 10),
            State = State.Repair,             // was Warning
            CurrentMileage = 120000,
            ExpectedLifetimeKm = 120000,
            ExpectedLifetimeYears = 6,
        };

        var bmwBattery = new VehicleComponent
        {
            VehicleId = bmw.VehicleId,
            ComponentType = ComponentType.Electrical,
            VehicleComponentBrand = "Varta",
            InstallationDate = new DateTime(2022, 9, 5),
            State = State.Normal,                   // was Monitor
            CurrentMileage = 155000,
            ExpectedLifetimeKm = 80000,
            ExpectedLifetimeYears = 5,
        };

        var bmwAirFilter = new VehicleComponent
        {
            VehicleId = bmw.VehicleId,
            ComponentType = ComponentType.Cooling,
            VehicleComponentBrand = "Mahle",
            InstallationDate = new DateTime(2024, 6, 1),
            State = State.Good,
            CurrentMileage = 175000,
            ExpectedLifetimeKm = 30000,
            ExpectedLifetimeYears = 3,
        };

        var bmwEngineOil = new VehicleComponent
        {
            VehicleId = bmw.VehicleId,
            ComponentType = ComponentType.Engine,
            VehicleComponentBrand = "Castrol",
            InstallationDate = new DateTime(2026, 4, 8),
            State = State.Good,
            CurrentMileage = 187320,
            ExpectedLifetimeKm = 15000,
            ExpectedLifetimeYears = 1,
        };

        // ── AUDI COMPONENTS ───────────────────────────────────────────────────
        var audiBrakePads = new VehicleComponent
        {
            VehicleId = audi.VehicleId,
            ComponentType = ComponentType.Brakes,
            VehicleComponentName = "Front Axle",
            VehicleComponentBrand = "ATE",
            InstallationDate = new DateTime(2023, 6, 20),
            State = State.Good,
            CurrentMileage = 45000,
            ExpectedLifetimeKm = 50000,
            ExpectedLifetimeYears = 5,
        };

        var audiOil = new VehicleComponent
        {
            VehicleId = audi.VehicleId,
            ComponentType = ComponentType.Engine,
            VehicleComponentBrand = "Mobil 1",
            InstallationDate = new DateTime(2025, 11, 15),
            State = State.Good,
            CurrentMileage = 57000,
            ExpectedLifetimeKm = 15000,
            ExpectedLifetimeYears = 1,
        };

        var audiTimingBelt = new VehicleComponent
        {
            VehicleId = audi.VehicleId,
            ComponentType = ComponentType.Engine,
            VehicleComponentBrand = "Continental",
            InstallationDate = new DateTime(2021, 8, 1),
            State = State.Normal,                   // was Monitor
            CurrentMileage = 10000,
            ExpectedLifetimeKm = 120000,
            ExpectedLifetimeYears = 6,
        };

        // ── HONDA COMPONENTS ──────────────────────────────────────────────────
        var hondaBrakePads = new VehicleComponent
        {
            VehicleId = honda.VehicleId,
            ComponentType = ComponentType.Brakes,
            VehicleComponentBrand = "Bosch",
            InstallationDate = new DateTime(2022, 4, 5),
            State = State.Critical,
            CurrentMileage = 255000,
            ExpectedLifetimeKm = 50000,
            ExpectedLifetimeYears = 5,
            Notes = "Due for replacement ASAP",
        };

        var hondaOil = new VehicleComponent
        {
            VehicleId = honda.VehicleId,
            ComponentType = ComponentType.Engine,
            VehicleComponentBrand = "Shell",
            InstallationDate = new DateTime(2025, 10, 3),
            State = State.Normal,                   // was Monitor
            CurrentMileage = 285000,
            ExpectedLifetimeKm = 15000,
            ExpectedLifetimeYears = 1,
        };

        _context.VehicleComponents.AddRange(
            bmwBrakePads, bmwTimingBelt, bmwBattery, bmwAirFilter, bmwEngineOil,
            audiBrakePads, audiOil, audiTimingBelt,
            hondaBrakePads, hondaOil
        );
        await _context.SaveChangesAsync();

        // ── BMW MAINTENANCE RECORDS ───────────────────────────────────────────
        var bmwOilChange = new MaintenanceRecord
        {
            VehicleId = bmw.VehicleId,
            ServiceDate = new DateTime(2026, 4, 8),
            ServiceType = ServiceType.Engine,
            ServiceName = "Full Oil Service",
            Cost = 320m,
            Description = "Castrol Edge 5W-30, 4.5L. Air filter replaced as well — was very dirty.",
            MaintenanceRecordComponents =
            [
                new()
                {
                    ComponentId = bmwEngineOil.VehicleComponentId,
                    ComponentChangeType = ComponentChangeType.Replaced,
                    WorkDescription = "Castrol Edge 5W-30 · 4.5L",
                    OldState = State.Critical,
                    NewState = State.Good,
                    PartsCost = 120m,
                    LaborCost = 80m,
                    TotalCost = 200m,
                    CreatedAt = DateTime.UtcNow,
                },
                new()
                {
                    ComponentId = bmwAirFilter.VehicleComponentId,
                    ComponentChangeType = ComponentChangeType.Replaced,
                    WorkDescription = "Mahle filter — very dirty",
                    OldState = State.Critical,
                    NewState = State.Good,
                    PartsCost = 55m,
                    LaborCost = 20m,
                    TotalCost = 75m,
                    CreatedAt = DateTime.UtcNow,
                }
            ]
        };

        var bmwBrakeService = new MaintenanceRecord
        {
            VehicleId = bmw.VehicleId,
            ServiceDate = new DateTime(2026, 3, 29),
            ServiceType = ServiceType.Brakes,       // was BrakeService
            ServiceName = "Brake Service",
            Cost = 1240m,
            Description = "Front brake pads and rotors replaced.",
            MaintenanceRecordComponents =
            [
                new()
                {
                    ComponentId = bmwBrakePads.VehicleComponentId,
                    ComponentChangeType = ComponentChangeType.Replaced,
                    WorkDescription = "Brembo front pads + rotors",
                    OldState = State.Critical,
                    NewState = State.Good,
                    PartsCost = 840m,
                    LaborCost = 400m,
                    TotalCost = 1240m,
                    CreatedAt = DateTime.UtcNow,
                }
            ]
        };

        var bmwInspection = new MaintenanceRecord
        {
            VehicleId = bmw.VehicleId,
            ServiceDate = new DateTime(2026, 2, 10),
            ServiceType = ServiceType.Inspection,
            ServiceName = "Annual Inspection",
            Cost = 180m,
            Description = "Full annual check. No critical issues found.",
        };

        var bmwTyreSwap = new MaintenanceRecord
        {
            VehicleId = bmw.VehicleId,
            ServiceDate = new DateTime(2025, 12, 3),
            ServiceType = ServiceType.Tyres,        // was TyreSwap
            ServiceName = "Winter Tyre Swap",
            Cost = 220m,
            Description = "Swapped to winter tyres. Stored summer set.",
        };

        // ── AUDI MAINTENANCE RECORDS ──────────────────────────────────────────
        var audiBrakeService = new MaintenanceRecord
        {
            VehicleId = audi.VehicleId,
            ServiceDate = new DateTime(2026, 3, 29),
            ServiceType = ServiceType.Brakes,       // was BrakeService
            ServiceName = "Brake Pads Replaced",
            Cost = 640m,
            Description = "Front brake pads replaced. Rotors still good.",
            MaintenanceRecordComponents =
            [
                new()
                {
                    ComponentId = audiBrakePads.VehicleComponentId,
                    ComponentChangeType = ComponentChangeType.Replaced,
                    WorkDescription = "ATE front pads",
                    OldState = State.Repair,  // was Warning
                    NewState = State.Good,
                    PartsCost = 340m,
                    LaborCost = 300m,
                    TotalCost = 640m,
                    CreatedAt = DateTime.UtcNow,
                }
            ]
        };

        // ── HONDA MAINTENANCE RECORDS ─────────────────────────────────────────
        var hondaInspection = new MaintenanceRecord
        {
            VehicleId = honda.VehicleId,
            ServiceDate = new DateTime(2026, 2, 10),
            ServiceType = ServiceType.Inspection,
            ServiceName = "Annual Inspection",
            Cost = 180m,
            Description = "Full inspection. Brake pads flagged as urgent.",
        };

        _context.MaintenanceRecords.AddRange(
            bmwOilChange, bmwBrakeService, bmwInspection, bmwTyreSwap,
            audiBrakeService,
            hondaInspection
        );
        await _context.SaveChangesAsync();

        // ── FUEL ENTRIES ──────────────────────────────────────────────────────
        _context.FuelEntries.AddRange(
            // BMW
            new FuelEntry { VehicleId = bmw.VehicleId, FuelType = FuelType.Diesel, RefillDate = new DateTime(2026, 4, 6), Amount = 45m, Cost = 280m, Mileage = 187300, Notes = "Orlen A4" },
            new FuelEntry { VehicleId = bmw.VehicleId, FuelType = FuelType.Diesel, RefillDate = new DateTime(2026, 3, 24), Amount = 50m, Cost = 310m, Mileage = 186850, Notes = "BP Autostrada" },
            new FuelEntry { VehicleId = bmw.VehicleId, FuelType = FuelType.Diesel, RefillDate = new DateTime(2026, 3, 9), Amount = 42m, Cost = 260m, Mileage = 186340, Notes = "Shell Kraków" },
            new FuelEntry { VehicleId = bmw.VehicleId, FuelType = FuelType.Diesel, RefillDate = new DateTime(2026, 2, 19), Amount = 48m, Cost = 295m, Mileage = 185800, Notes = "Orlen Warszawa" },
            new FuelEntry { VehicleId = bmw.VehicleId, FuelType = FuelType.Diesel, RefillDate = new DateTime(2026, 1, 30), Amount = 44m, Cost = 272m, Mileage = 185200, Notes = "Circle K" },
            // Audi
            new FuelEntry { VehicleId = audi.VehicleId, FuelType = FuelType.Diesel, RefillDate = new DateTime(2026, 4, 10), Amount = 40m, Cost = 248m, Mileage = 64450, Notes = "Orlen" },
            new FuelEntry { VehicleId = audi.VehicleId, FuelType = FuelType.Diesel, RefillDate = new DateTime(2026, 3, 18), Amount = 38m, Cost = 235m, Mileage = 63900, Notes = "BP" },
            // Honda                                                  // was Petrol
            new FuelEntry { VehicleId = honda.VehicleId, FuelType = FuelType.Petrol95, RefillDate = new DateTime(2026, 4, 12), Amount = 35m, Cost = 210m, Mileage = 297800, Notes = "Shell" },
            new FuelEntry { VehicleId = honda.VehicleId, FuelType = FuelType.Petrol95, RefillDate = new DateTime(2026, 3, 28), Amount = 33m, Cost = 198m, Mileage = 297400, Notes = "Orlen" }
        );
        await _context.SaveChangesAsync();

        // ── PREDICTIONS ───────────────────────────────────────────────────────
        _context.Predictions.AddRange(
            new Prediction
            {
                VehicleId = bmw.VehicleId,
                VehicleComponentId = bmwBrakePads.VehicleComponentId,
                Name = "Brake Pads",
                ComponentType = ComponentType.Brakes,
                PredictedServiceDate = new DateTime(2026, 4, 20),
                ConfidenceScore = 0.91f,
                Status = PredictionStatus.Active,
                CreatedAt = DateTime.UtcNow,
            },
            new Prediction
            {
                VehicleId = bmw.VehicleId,
                VehicleComponentId = bmwTimingBelt.VehicleComponentId,
                Name = "Timing Belt",
                ComponentType = ComponentType.Engine,
                PredictedServiceDate = new DateTime(2026, 8, 1),
                ConfidenceScore = 0.74f,
                Status = PredictionStatus.Active,
                CreatedAt = DateTime.UtcNow,
            },
            new Prediction
            {
                VehicleId = bmw.VehicleId,
                VehicleComponentId = bmwEngineOil.VehicleComponentId,
                Name = "Oil Change",
                ComponentType = ComponentType.Engine,
                PredictedServiceDate = new DateTime(2026, 4, 8),
                ConfidenceScore = 0.99f,
                Status = PredictionStatus.Completed,
                CompletedAt = new DateTime(2026, 4, 8),
                CreatedAt = DateTime.UtcNow,
            },
            new Prediction
            {
                VehicleId = bmw.VehicleId,
                VehicleComponentId = bmwBattery.VehicleComponentId,
                Name = "Battery Check",
                ComponentType = ComponentType.Electrical,
                PredictedServiceDate = new DateTime(2026, 9, 15),
                ConfidenceScore = 0.58f,
                Status = PredictionStatus.Ignored,
                CreatedAt = DateTime.UtcNow,
            },
            new Prediction
            {
                VehicleId = honda.VehicleId,
                VehicleComponentId = hondaBrakePads.VehicleComponentId,
                Name = "Brake Pads",
                ComponentType = ComponentType.Brakes,
                PredictedServiceDate = new DateTime(2026, 5, 1),
                ConfidenceScore = 0.88f,
                Status = PredictionStatus.Active,
                CreatedAt = DateTime.UtcNow,
            }
        );
        await _context.SaveChangesAsync();
    }
}