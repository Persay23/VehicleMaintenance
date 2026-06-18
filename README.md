# AutoCare — Backend

REST API for a mobile-first vehicle maintenance tracking application, built as a diploma project. Handles user management, vehicle and component tracking, maintenance records, fuel entries, cost summaries, general expenses, driving profiles, and AI-powered service predictions and diagnostics via Google Gemini.

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
| AI | Google Gemini REST API (`gemini-2.5-flash`, via `HttpClient`) |
| API docs | Swagger / Swashbuckle |

---

## Features

- Cookie-based auth — register, login, logout via ASP.NET Core Identity
- Full vehicle CRUD with make, model, year, engine, fuel type, transmission, and odometer
- Vehicle component tracking with expected lifetime (km + years) and health calculation
- `ComponentHealthCalculator` as the single source of truth for all health math — used by the component service, AI service, and prompt builder
- Maintenance records with per-component cost and labor breakdown via a bridge table
- Fuel/liquid entry log with type, amount, cost, mileage, and station notes
- General expenses tracking — one-off and recurring costs (insurance, tax, fines, etc.) with recurrence scheduling
- User driving profile — annual km, typical trip distance, highway ratio, driving style
- Monthly cost summaries and full event timeline per vehicle
- AI-generated per-component service predictions — estimated next service date, remaining km, health score, recommendation, and confidence score capped by maintenance history depth
- AI-generated vehicle-level suggestions — up to five ranked recommendations derived from the vehicle's component set and recent records
- AI symptom diagnosis — structured differential with likely causes, urgency, recommended actions, and affected components
- AI calls run through a dedicated `GeminiService` abstraction; all prompt construction is handled by a `PromptBuilderService` split into domain-specific partial classes

---

## Architecture

The project follows a **Controller → Service → EF Core** layered pattern. Each domain has a dedicated service class, interface, and set of DTOs. AutoMapper translates between entity and DTO layers — controllers never return entities directly.

```
VehicleMaintenance/
├── Controllers/
│   ├── AuthController.cs
│   ├── UserController.cs
│   ├── VehicleController.cs
│   ├── VehicleComponentController.cs
│   ├── MaintenanceRecordController.cs
│   ├── MaintenanceRecordComponentController.cs
│   ├── FuelEntryController.cs
│   ├── PredictionController.cs
│   ├── GeneralExpenseController.cs
│   ├── UserDrivingProfileController.cs
│   └── AIController.cs
├── Services/
│   ├── Interfaces/
│   ├── VehicleService.cs
│   ├── VehicleComponentService.cs
│   ├── MaintenanceRecordService.cs
│   ├── MaintenanceRecordComponentService.cs
│   ├── FuelEntryService.cs
│   ├── PredictionService.cs
│   ├── GeneralExpenseService.cs
│   ├── UserDrivingProfileService.cs
│   ├── UserService.cs
│   └── AI/
│       ├── IAiPredictionService.cs
│       ├── AiPredictionService.cs  # background updates, predictions, suggestions, diagnosis
│       ├── IGeminiService.cs
│       ├── GeminiService.cs        # direct Gemini REST API client
│       └── Prompts/
│           ├── PromptBuilderService.cs             # shared helpers + vehicle summary
│           ├── PromptBuilderService.Prediction.cs  # per-component prediction prompt
│           ├── PromptBuilderService.Suggestions.cs # vehicle-level suggestions prompt
│           └── PromptBuilderService.Diagnosis.cs   # symptom diagnosis prompt
├── Models/
│   ├── ComponentHealthCalculator.cs  # single source of truth — Compute() → ComponentMeasurements
│   ├── ComponentStateCalculator.cs   # DeriveState() — maps km/age usage to state label
│   ├── Entities/                     # EF Core entity classes
│   ├── Enums/                        # ComponentType, ServiceType, PredictionStatus, etc.
│   └── AI/
│       ├── AiPredictionResult.cs
│       ├── AiSuggestion.cs
│       └── AiDiagnosisResult.cs
├── DTOs/
│   ├── AI/                           # DiagnoseResponseDto
│   ├── Auth/
│   ├── Users/
│   ├── Vehicles/
│   ├── VehicleComponents/
│   ├── MaintenanceRecords/
│   ├── MaintenanceRecordComponents/
│   ├── FuelEntry/
│   ├── Prediction/
│   ├── GeneralExpense/
│   └── UserDrivingProfile/
├── Data/
│   └── AppDbContext.cs
├── Mappings/
│   └── MappingProfile.cs
└── Migrations/
```

---

## Database Schema

```
USER (IdentityUser)
 ├── USER_DRIVING_PROFILE (1:1)
 └── VEHICLE (1:N)
      ├── VEHICLE_COMPONENT (1:N)
      │    └── MAINTENANCE_RECORD_COMPONENT (bridge, N:M)
      ├── MAINTENANCE_RECORD (1:N)
      │    └── MAINTENANCE_RECORD_COMPONENT (1:N)
      ├── LIQUID_ENTRY (1:N)
      ├── PREDICTION (1:N)
      └── GENERAL_EXPENSE (1:N)
```

**Key design decisions:**
- `User.Id` is a GUID string inherited from `IdentityUser` — no separate users table PK
- All cost/decimal fields use `precision(18, 2)`
- Component health is the **worst-of-two**: `Min(kmLifetimePercent, yearsLifetimePercent)` — a component at 90% km life but only 20% year life is at 20% health
- Maintenance records and components are linked through `MaintenanceRecordComponent`, which carries individual `laborCost`, `partsCost`, `otherCost`, and `technicianName` — one visit can cover multiple components with separate cost breakdowns
- AI results are stored directly on `VehicleComponent` (denormalized) to avoid joins on the hot path; `AiGeneratedAt` is used as a staleness guard
- Predictions are vehicle-level suggestions (not component predictions) and live in a separate `Prediction` table

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MS SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code with C# extension
- A Google Gemini API key (free tier at [aistudio.google.com](https://aistudio.google.com))

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/Persay23/VehicleMaintenance.git
cd VehicleMaintenance
```

**2. Configure the connection string and AI key**

Edit `VehicleMaintenance/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=VehicleMaintenanceDb;Trusted_Connection=True;TrustServerCertificate=True"
},
"AiService": {
  "ApiKey": "<your-gemini-api-key>",
  "Model": "gemini-2.5-flash"
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

All endpoints require authentication (session cookie) unless marked **—**.

### Auth

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|:----:|
| POST | `/api/auth/register` | Create account | — |
| POST | `/api/auth/login` | Login (sets session cookie) | — |
| POST | `/api/auth/logout` | Invalidate session | ✓ |

### Users

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users/me` | Current user (session check) |
| GET | `/api/users/{id}` | Get user by ID |
| PATCH | `/api/users/{id}` | Update profile |
| DELETE | `/api/users/{id}` | Delete account |
| POST | `/api/users/{id}/change-password` | Change password |

### Vehicles

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/vehicles` | All vehicles for current user |
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
| GET | `/api/vehiclecomponents/{id}/history` | Service history for a component |

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
| GET | `/api/predictions/vehicle/{vehicleId}` | Active predictions for vehicle |
| GET | `/api/predictions/{id}` | Prediction by ID |
| POST | `/api/predictions` | Create prediction |
| PATCH | `/api/predictions/{id}` | Update status (Active / Completed / Ignored) |
| DELETE | `/api/predictions/{id}` | Delete prediction |

### General Expenses

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/generalexpense/vehicle/{vehicleId}` | Expenses for a vehicle |
| GET | `/api/generalexpense/user/{userId}` | All expenses for a user |
| GET | `/api/generalexpense/{id}` | Expense by ID |
| POST | `/api/generalexpense` | Create expense |
| PATCH | `/api/generalexpense/{id}` | Update expense |
| DELETE | `/api/generalexpense/{id}` | Delete expense |

### Driving Profile

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/userdrivingprofile/{userId}` | Get profile |
| POST | `/api/userdrivingprofile` | Create profile |
| PATCH | `/api/userdrivingprofile/{userId}` | Update profile |

### AI

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/ai/predict/{componentId}` | Force-regenerate component AI prediction |
| POST | `/api/ai/suggestions/{vehicleId}` | Force-regenerate vehicle-level AI suggestions |
| POST | `/api/ai/diagnose/{vehicleId}` | Diagnose symptom — returns structured differential |
| GET | `/api/ai/diagnose/{vehicleId}` | Diagnosis history for a vehicle |

---

## Authentication

ASP.NET Core Identity with **cookie-based sessions** — no JWT. The frontend sends `credentials: include` on every request. On login, Identity sets a `.AspNetCore.Identity.Application` cookie that the browser returns on subsequent requests.

CORS is configured to allow `http://localhost:5173` (the Vite dev server) with `AllowCredentials`.

Email confirmation and password-reset emails are disabled (`NoOpEmailSender`) — the flows exist in the Identity scaffolding but do not send real emails.

---

## Health Calculation

Component health is computed by `ComponentHealthCalculator.Compute(component, currentMileage)`, which returns a `ComponentMeasurements` record — the single source of truth used by every service that needs health data.

Each component exposes:
- `KmUsed` / `DaysUsed` — usage since installation
- `RemainingKm` — km left before expected replacement
- `KmRemainingPercent` / `YearsRemainingPercent` — percentage lifetime remaining for each dimension
- `State` — derived label based on the lower of the two percentages

| State | Remaining health |
|---|---|
| Perfect | > 75% |
| Good | 51 – 75% |
| Normal | 31 – 50% |
| Repair | 16 – 30% |
| Critical | ≤ 15% |

`ComponentStateCalculator.DeriveState` encapsulates the threshold logic so the same rules apply everywhere: the component service, the prompt builder, and the AI service all call the same code.

---

## AI Integration

AI features are handled by `AiPredictionService`, which depends on `IGeminiService` (a thin `HttpClient` wrapper over the Gemini REST API) and `PromptBuilderService` (a static class split into domain partial files).

**Per-component predictions** (`GenerateComponentPredictionAsync`):
- Builds a prompt from the component's health, maintenance history, vehicle profile, and driving data
- Parses the model response into `AiPredictionResult` (date, km estimate, health score, recommendation, reasoning)
- Caps the raw confidence score based on history depth (0 records → max 40%, 1 → 60%, 2 → 75%, 3+ → 85%) and adjusts for manual schedule signals and driving profile presence
- Stores results directly on `VehicleComponent`; re-runs are guarded by a 24-hour staleness window unless `forceRefresh = true`

**Vehicle-level suggestions** (`GenerateVehicleSuggestionsAsync`):
- Summarises all components, recent records, and the driving profile into a single prompt
- Returns up to 5 prioritised suggestions (`AiSuggestion` list), each validated to belong to the correct vehicle
- Protected by a 10-minute cooldown to prevent duplicate runs when multiple components are saved in one action

**Diagnosis** (`DiagnoseAsync`):
- Accepts a free-text symptom string
- Returns a `DiagnoseResponseDto` with likely causes, urgency level, urgency explanation, recommended actions, related components, and a standard disclaimer
- A trigger in `AIController` fires background component + suggestion updates automatically after each diagnosis

---

## License

Created as a diploma submission. Educational use only.
