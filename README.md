# AutoCare — Backend

REST API for a mobile-first vehicle maintenance tracking application, built as a diploma project. Handles user management, vehicle and component tracking, maintenance records, fuel entries, cost summaries, and service predictions.

**Frontend repo:** [autocare-frontend](https://github.com/Persay23/autocare-frontend)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 10 |
| Web framework | ASP.NET Core Web API |
| Authentication | ASP.NET Core Identity (cookie-based) |
| ORM | Entity Framework Core |
| Database | MS SQL Server |
| Object mapping | AutoMapper 14 |
| API docs | Swagger / Swashbuckle |

---

## Features

- Cookie-based auth — register, login, logout via ASP.NET Core Identity
- Full vehicle CRUD with make, model, year, engine, fuel type, transmission, and odometer
- Vehicle component tracking with expected lifetime (km + years) and health calculation
- Maintenance records with per-component cost and labor breakdown via a bridge table
- Fuel/liquid entry log with type, amount, cost, mileage, and station notes
- Monthly cost summaries and full event timeline per vehicle
- Service predictions with confidence scoring and status tracking (Active / Completed / Ignored)
- Date-range and type filtering on records and liquid entries

---

## Architecture

The project follows a **Controller → Service → EF Core** layered pattern. Each domain has a dedicated service class, interface, and set of DTOs. AutoMapper translates between entity and DTO layers — controllers never return entities directly.

```
VehicleMaintenance/
├── Controllers/              # HTTP layer — thin, delegates to services
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── VehiclesController.cs
│   ├── VehicleComponentsController.cs
│   ├── MaintenanceRecordsController.cs
│   ├── MaintenanceRecordComponentsController.cs
│   ├── LiquidEntriesController.cs (fuel)
│   └── PredictionsController.cs
├── Services/                 # Business logic
│   ├── Interfaces/
│   ├── VehicleService.cs
│   ├── VehicleComponentService.cs
│   ├── MaintenanceRecordService.cs
│   ├── MaintenanceRecordComponentService.cs
│   ├── FuelEntryService.cs
│   ├── PredictionService.cs
│   └── UserService.cs
├── Models/
│   ├── Entities/             # EF Core entity classes
│   └── Enums/                # Shared enums (ServiceType, ComponentType, etc.)
├── DTOs/                     # Request/response shapes, one folder per domain
│   ├── Auth/
│   ├── Users/
│   ├── Vehicles/
│   ├── VehicleComponents/
│   ├── MaintenanceRecords/
│   ├── MaintenanceRecordComponents/
│   ├── LiquidEntries/
│   └── Predictions/
├── Data/
│   └── AppDbContext.cs       # EF Core DbContext + Identity
├── Mappings/
│   └── MappingProfile.cs     # All AutoMapper profiles in one place
└── Migrations/               # EF Core migration history
```

---

## Database Schema

```
USER (IdentityUser)
 └── VEHICLE (1:N)
      ├── VEHICLE_COMPONENT (1:N)
      │    └── MAINTENANCE_RECORD_COMPONENT (bridge, N:M)
      ├── MAINTENANCE_RECORD (1:N)
      │    └── MAINTENANCE_RECORD_COMPONENT (1:N)
      ├── LIQUID_ENTRY (1:N)
      └── PREDICTION (1:N)
```

**Key design decisions:**
- `User.Id` is a GUID string inherited from `IdentityUser` — no separate users table primary key
- All cost/decimal fields use `precision(18, 2)`
- Component health is the **worst-of-two**: `Min(kmLifetimePercent, yearsLifetimePercent)` — a component that has covered 90% of its expected km but is only 20% through its expected years is at 20% health
- Maintenance records and components are linked through `MaintenanceRecordComponent`, which carries individual `laborCost`, `partsCost`, and `technicianName` — one visit can cover multiple components with separate cost breakdowns

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MS SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code with C# extension

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/Persay23/VehicleMaintenance.git
cd VehicleMaintenance
```

**2. Configure the connection string**

Edit `VehicleMaintenance/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=VehicleMaintenanceDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

**3. Apply migrations**
```bash
cd VehicleMaintenance
dotnet ef database update
```

**4. Run**
```bash
dotnet run
```

**5. Open Swagger**
```
https://localhost:7235/swagger
```

---

## API Reference

### Auth

| Method | Endpoint | Description | Auth required |
|--------|----------|-------------|:---:|
| POST | `/api/auth/register` | Create account | — |
| POST | `/api/auth/login` | Login (sets session cookie) | — |
| POST | `/api/auth/logout` | Invalidate session | ✓ |

### Users

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users/me` | Get current user (used for session check) |
| GET | `/api/users/{id}` | Get user by ID |
| PATCH | `/api/users/{id}` | Update profile |
| DELETE | `/api/users/{id}` | Delete account |
| POST | `/api/users/{id}/change-password` | Change password |

### Vehicles

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/vehicles` | All vehicles for authenticated user |
| GET | `/api/vehicles/{id}` | Vehicle by ID |
| POST | `/api/vehicles` | Add vehicle |
| PATCH | `/api/vehicles/{id}` | Update vehicle |
| DELETE | `/api/vehicles/{id}` | Delete vehicle |
| GET | `/api/vehicles/{id}/summary/costs` | Monthly cost breakdown (`?from=&to=`) |
| GET | `/api/vehicles/{id}/summary/timeline` | Full event timeline |

### Vehicle Components

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/vehiclecomponents/vehicle/{vehicleId}` | All components for vehicle |
| GET | `/api/vehiclecomponents/{id}` | Component by ID |
| POST | `/api/vehiclecomponents` | Add component |
| PATCH | `/api/vehiclecomponents/{id}` | Update component |
| DELETE | `/api/vehiclecomponents/{id}` | Delete component |
| GET | `/api/vehiclecomponents/vehicle/{vehicleId}/health` | Health data for all components |

### Maintenance Records

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/maintenancerecords/vehicle/{vehicleId}` | Records (`?fromDate=&toDate=&serviceType=`) |
| GET | `/api/maintenancerecords/{id}` | Record by ID |
| POST | `/api/maintenancerecords` | Create record |
| PATCH | `/api/maintenancerecords/{id}` | Update record |
| DELETE | `/api/maintenancerecords/{id}` | Delete record |

### Maintenance Record Components (bridge)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/maintenancerecordcomponents/record/{recordId}` | Components for a record |
| POST | `/api/maintenancerecordcomponents` | Link component to record |
| PATCH | `/api/maintenancerecordcomponents/{id}` | Update link entry |
| DELETE | `/api/maintenancerecordcomponents/{id}` | Remove link |

### Liquid Entries (Fuel & Fluids)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/liquidentries/vehicle/{vehicleId}` | Entries (`?liquidType=&fromDate=&toDate=`) |
| GET | `/api/liquidentries/{id}` | Entry by ID |
| POST | `/api/liquidentries` | Log refill |
| PATCH | `/api/liquidentries/{id}` | Update entry |
| DELETE | `/api/liquidentries/{id}` | Delete entry |

### Predictions

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/predictions/vehicle/{vehicleId}` | Predictions for vehicle |
| GET | `/api/predictions/{id}` | Prediction by ID |
| POST | `/api/predictions` | Create prediction |
| PATCH | `/api/predictions/{id}` | Update status (Active / Completed / Ignored) |
| DELETE | `/api/predictions/{id}` | Delete prediction |

---

## Authentication

ASP.NET Core Identity with **cookie-based sessions** — no JWT. The frontend sends `credentials: include` on every request. On login, Identity sets a `.AspNetCore.Identity.Application` cookie that the browser returns on subsequent requests.

CORS is configured to allow `http://localhost:5173` (the Vite dev server) with `AllowCredentials`.

Email confirmation and password-reset emails are disabled (`NoOpEmailSender`) — the flows exist in the Identity scaffolding but do not send real emails.

---

## Health Calculation

Component health is exposed through `GET /api/vehiclecomponents/vehicle/{vehicleId}/health`.

Each component returns:
- `kmLifetimePercent` — percentage of expected km lifetime remaining
- `yearsLifetimePercent` — percentage of expected year lifetime remaining
- `remainingKm` — km left before expected replacement
- `status` — `Good` / `Monitor` / `Warning` / `Critical` based on the lower of the two percentages

| Status | Health % |
|---|---|
| Critical | ≤ 15% |
| Warning | 16–30% |
| Monitor | 31–50% |
| Good | 51–74% |
| Perfect | ≥ 75% |

---

## Known Limitations

- Prediction confidence scores and dates are currently set manually — automated inference from maintenance history is planned
- Repository pattern is planned but not yet implemented — services access `AppDbContext` directly
- No refresh-token mechanism — sessions expire based on the Identity cookie lifetime

---

## License

Created as a diploma submission. Educational use only.
