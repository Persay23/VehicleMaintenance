# VehicleMaintenance

A full-stack vehicle maintenance tracking application built as a diploma project. The system allows users to manage their vehicles, track components health, log maintenance records, monitor fluid refills, and receive AI-powered service predictions.

---

## Tech Stack

**Backend**
- .NET 10 — Web API
- ASP.NET Core Identity — authentication & user management
- Entity Framework Core — ORM
- MS SQL Server — database
- AutoMapper 14 — object mapping

**Frontend** *(planned)*
- React + Vite
- Axios
- React Router

---

## Features

- User registration and login via ASP.NET Core Identity
- Full vehicle management (brand, model, year, engine, fuel, transmission, mileage)
- Component tracking with health calculation based on mileage and age
- Maintenance records with detailed per-component work logging (costs, labor, technician, shop)
- Fluid/liquid entry tracking (fuel, oil, coolant, brake fluid, etc.)
- Cost summary and timeline per vehicle
- Service predictions with confidence scoring
- Filtering on maintenance records and liquid entries by date range and type
- Component health status — Good / Monitor / Warning / Critical

---

## Project Structure

```
VehicleMaintenance/
├── Controllers/
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── VehiclesController.cs
│   ├── VehicleComponentsController.cs
│   ├── MaintenanceRecordsController.cs
│   ├── MaintenanceRecordComponentsController.cs
│   ├── LiquidEntriesController.cs
│   └── PredictionsController.cs
├── Services/
│   ├── Interfaces/
│   └── *.cs
├── Models/
│   ├── Entities/
│   └── Enums/
├── DTOs/
│   ├── Auth/
│   ├── Users/
│   ├── Vehicles/
│   ├── VehicleComponents/
│   ├── MaintenanceRecords/
│   ├── MaintenanceRecordComponents/
│   ├── LiquidEntries/
│   └── Predictions/
├── Data/
│   └── AppDbContext.cs
├── Mappings/
│   └── MappingProfile.cs
├── Migrations/
└── Pages/
    └── Shared/
        ├── _Layout.cshtml
        └── _LoginPartial.cshtml
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [MS SQL Server](https://www.microsoft.com/en-us/sql-server) or SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/your-username/VehicleMaintenance.git
cd VehicleMaintenance
```

**2. Configure the database connection**

Update `appsettings.json` with your SQL Server connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=VehicleMaintenanceDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

**3. Apply migrations**
```bash
dotnet ef database update
```

**4. Run the application**
```bash
dotnet run
```

**5. Open Swagger UI**
```
https://localhost:7235/swagger
```

---

## API Overview

### Auth
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/auth/register` | Register new account | Public |
| POST | `/api/auth/login` | Login | Public |
| POST | `/api/auth/logout` | Logout | Authenticated |

### Users
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users` | Get all users |
| GET | `/api/users/{id}` | Get user by ID |
| PATCH | `/api/users/{id}` | Update user profile |
| DELETE | `/api/users/{id}` | Delete user account |
| POST | `/api/users/{id}/change-password` | Change password |

### Vehicles
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/vehicles` | Get all vehicles for user |
| GET | `/api/vehicles/{id}` | Get vehicle by ID |
| POST | `/api/vehicles` | Add new vehicle |
| PATCH | `/api/vehicles/{id}` | Update vehicle |
| DELETE | `/api/vehicles/{id}` | Delete vehicle |
| GET | `/api/vehicles/{id}/summary/costs` | Cost summary (supports `?from=&to=`) |
| GET | `/api/vehicles/{id}/summary/timeline` | Full event timeline |

### Vehicle Components
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/vehiclecomponents/vehicle/{vehicleId}` | Get all components |
| GET | `/api/vehiclecomponents/{id}` | Get component by ID |
| POST | `/api/vehiclecomponents` | Add component |
| PATCH | `/api/vehiclecomponents/{id}` | Update component |
| DELETE | `/api/vehiclecomponents/{id}` | Delete component |
| GET | `/api/vehiclecomponents/vehicle/{vehicleId}/health` | Component health status |

### Maintenance Records
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/maintenancerecords/vehicle/{vehicleId}` | Get records (supports `?fromDate=&toDate=&serviceType=`) |
| GET | `/api/maintenancerecords/{id}` | Get record by ID |
| POST | `/api/maintenancerecords` | Create record |
| PATCH | `/api/maintenancerecords/{id}` | Update record |
| DELETE | `/api/maintenancerecords/{id}` | Delete record |

### Liquid Entries
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/liquidentries/vehicle/{vehicleId}` | Get entries (supports `?liquidType=&fromDate=&toDate=`) |
| GET | `/api/liquidentries/{id}` | Get entry by ID |
| POST | `/api/liquidentries` | Log refill |
| PATCH | `/api/liquidentries/{id}` | Update entry |
| DELETE | `/api/liquidentries/{id}` | Delete entry |

### Predictions
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/predictions/vehicle/{vehicleId}` | Get predictions for vehicle |
| GET | `/api/predictions/{id}` | Get prediction by ID |
| POST | `/api/predictions` | Generate prediction |
| PATCH | `/api/predictions/{id}` | Update prediction |
| DELETE | `/api/predictions/{id}` | Delete prediction |

---

## Database Schema

```
USER (IdentityUser)
 └── VEHICLE (1:N)
      ├── VEHICLE_COMPONENT (1:N)
      │    └── MAINTENANCE_RECORD_COMPONENT (N:M bridge)
      ├── MAINTENANCE_RECORD (1:N)
      │    └── MAINTENANCE_RECORD_COMPONENT (1:N)
      ├── LIQUID_ENTRY (1:N)
      └── PREDICTION (1:N)
```

Key design decisions:
- `User.Id` is a GUID string inherited from ASP.NET Core `IdentityUser`
- All decimal cost fields use `precision(18,2)`
- Component health is calculated from both mileage used and years since installation — whichever is worse determines the status
- Maintenance records and components are linked through a `MaintenanceRecordComponent` bridge table, allowing one service visit to cover multiple components with individual cost and labor breakdowns

---

## Authentication

The project uses **ASP.NET Core Identity** with cookie-based authentication. Passwords are hashed automatically by Identity — no manual hashing.

Built-in Identity UI pages are available at:
- `/Identity/Account/Login`
- `/Identity/Account/Register`

API endpoints for frontend use are at `/api/auth/login` and `/api/auth/register`.

---

## Frontend (Planned)

The frontend will be built separately as a React + Vite application:

```bash
npm create vite@latest vehicle-maintenance-front -- --template react
cd vehicle-maintenance-front
npm install axios react-router-dom
```

CORS is pre-configured on the backend to allow requests from `http://localhost:5173`.

---

## Known Limitations

- Email sending is disabled (`NoOpEmailSender`) — forgot password and email confirmation flows do not send real emails
- Prediction logic is currently a placeholder — confidence scoring and date calculation will be implemented in a future iteration
- Repository pattern is planned but not yet implemented — services currently interact with `AppDbContext` directly

---

## License

This project was created as a diploma submission and is intended for educational purposes.
