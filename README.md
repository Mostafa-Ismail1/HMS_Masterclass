# Hospital Management System (HMS)

Enterprise-grade Hospital Management Backend built with ASP.NET Core (.NET 9), Clean Architecture, CQRS, MediatR, Entity Framework Core, Dapper, and SQL Server.

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C# 13](https://img.shields.io/badge/C%23-13.0-239120?style=flat-square&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20Onion-blue?style=flat-square)](https://blog.cleancoder.com/)
[![Pattern](https://img.shields.io/badge/Pattern-CQRS%20with%20MediatR-purple?style=flat-square)](https://github.com/jbogard/MediatR)
[![ORM](https://img.shields.io/badge/ORM-EF%20Core%20%2B%20Dapper-green?style=flat-square)](https://github.com/DapperLib/Dapper)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC292B?style=flat-square&logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-black?style=flat-square)](LICENSE)

---

## Architecture Overview

The solution adheres strictly to **Clean Architecture** principles, enforcing directional dependencies pointing inward toward the domain core.

```mermaid
graph TD
    subgraph Presentation["Presentation Layer (HMS.Api)"]
        Controllers["API Controllers (MediatR ISender)"]
        Middlewares["Global Exception Handler & Rate Limiter"]
        Doc["OpenAPI / Swagger / Scalar"]
    end

    subgraph Application["Application Layer (HMS.Application)"]
        Commands["Commands (CQRS Write Side)"]
        Queries["Queries (CQRS Read Side via Dapper)"]
        Behaviors["Validation Pipeline Behaviors (FluentValidation)"]
        Interfaces["Repository & Connection Interfaces"]
    end

    subgraph Domain["Core Domain Layer (HMS.Domain)"]
        Entities["Rich Domain Entities (Private Setters)"]
        VO["Value Objects (PhoneNumber)"]
        Events["Domain Events (AppointmentBooked)"]
        Result["Result Pattern (Error Handling)"]
    end

    subgraph Infrastructure["Infrastructure Layer (HMS.Infrastructure)"]
        EF["ApplicationDbContext (EF Core 9)"]
        Dapper["Dapper Micro-ORM (ISqlConnectionFactory)"]
        Configs["17 Fluent Mappings (Temporal, Triggers, Keys)"]
        Repos["Repository Implementations"]
    end

    Presentation --> Application
    Application --> Domain
    Infrastructure --> Application
    Infrastructure --> Domain
    Presentation --> Infrastructure
```

---

## Solution Structure

```
HMS_Masterclass/
├── HMS.Domain/                # Pure domain core (zero external dependencies)
│   ├── Common/                # Entity<TId>, ValueObject, Result<T>, Error, IDomainEvent
│   ├── Entities/              # 19 Rich Entities grouped by business subdomain
│   └── ValueObjects/          # Self-validating Value Objects (e.g. PhoneNumber)
│
├── HMS.Application/           # Use cases, CQRS commands & queries
│   ├── Common/                # Interfaces, DTOs, ValidationBehavior pipeline
│   ├── Appointments/          # Book, Cancel, Reschedule, GetAppointments
│   ├── Patients/              # RegisterPatient, GetPatients (Dapper)
│   ├── Departments/           # CreateDepartment, GetDepartments
│   ├── Doctors/               # CreateDoctor, GetDoctors
│   ├── Medications/           # CreateMedication, GetMedications
│   ├── Prescriptions/         # IssuePrescription, GetPrescriptions
│   ├── LabOrders/             # CreateLabOrder, RecordLabResult, GetLabOrders
│   ├── Admissions/            # AdmitPatient, DischargePatient, GetRooms
│   ├── Invoices/              # CreateInvoice, PayInvoice, GetInvoices
│   ├── Insurances/            # CreateInsuranceProvider, GetInsuranceProviders
│   └── Staff/                 # CreateStaff, GetStaff, GetNurses
│
├── HMS.Infrastructure/        # Data persistence & external concerns
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs     # Unified EF Core context for all 19 tables
│   │   ├── SqlConnectionFactory.cs     # ADO.NET connection factory for Dapper queries
│   │   ├── Configurations/             # 17 Fluent mappings (temporal tables, triggers)
│   │   └── Repositories/               # EF Core repository implementations
│   └── DependencyInjection.cs          # Infrastructure service registration
│
└── HMS.Api/                   # Thin presentation host
    ├── Controllers/           # 11 Controllers delegating to MediatR ISender
    ├── BackgroundServices/    # HospitalMetricsBackgroundService (PeriodicTimer)
    ├── Middlewares/           # GlobalExceptionHandler (RFC 9457 Problem Details)
    ├── Program.cs             # Composition root
    └── HMS.Api.http           # Executable HTTP integration test suite
```

---

## Core Domain Entities (19 Tables)

| Business Subdomain | Database Schema | Tables / Entities |
|--------------------|-----------------|-------------------|
| Clinical Core | `clinical` | `Patients` (Temporal Table with History), `Appointments` |
| Clinical Services | `clinical` | `Prescriptions`, `PrescriptionDetails`, `Medications`, `LabOrders`, `LabTests` |
| Inpatient Care | `clinical` / `hr` | `Admissions`, `Rooms` |
| Human Resources | `hr` | `Departments`, `Staff`, `Doctors`, `DoctorSchedules`, `Nurses` |
| Financial & Billing | `billing` | `Invoices` (Partitioned), `InvoiceItems`, `InvoiceStatusLogs` (Audit Triggers), `InsuranceProviders`, `PatientInsurance` |

---

## Key Technical Features

### 1. Hybrid Persistence Strategy (CQRS)
- **Write Side (Commands):** Handled through Entity Framework Core 9 to preserve domain encapsulation, invariants, and change tracking across rich entities.
- **Read Side (Queries):** Executed through **Dapper** via `ISqlConnectionFactory` for ultra-low latency, direct raw SQL performance, and zero ORM tracking overhead.

### 2. Functional Result Pattern
- Eliminates unhandled domain exceptions. Business validation errors return typed `Result<T>` records with structured `Error` objects (`Validation`, `NotFound`, `Conflict`, `Failure`).

### 3. Automatic Validation Pipeline
- MediatR open pipeline behavior (`ValidationBehavior<TRequest, TResponse>`) automatically scans and validates requests with FluentValidation before reaching domain handlers.

### 4. API Versioning
- Configured with `Asp.Versioning.Mvc` supporting multiple resolution strategies:
  - URL Path Segment: `/api/v1/patients`
  - Query Parameter: `/api/patients?api-version=1.0`
  - Request Header: `X-Version: 1.0`

### 5. Enterprise Security & Performance
- **Authentication & Authorization:** JWT Bearer authentication with refresh token rotation and claim/role-based authorization policies.
- **Performance & Caching:** .NET 9 HybridCache, OutputCache, fixed-window rate limiting, and Brotli/Gzip response compression.
- **Observability:** Structured logging with Serilog (Console & rolling file sinks) and SQL Server Health Checks.
- **Documentation:** Interactive OpenAPI specification accessible via SwaggerUI and Scalar.

---

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- Microsoft SQL Server 2022+ (Local or Express)
- Visual Studio 2022 (v17.12+) or VS Code with C# Dev Kit

### Setup & Run
1. Clone the repository:
   ```bash
   git clone https://github.com/Mostafa-Ismail1/HMS_Masterclass.git
   cd HMS_Masterclass
   ```

2. Update database connection string in `HMS.Api/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=HMS_DB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. Build and run:
   ```bash
   dotnet build
   dotnet run --project HMS.Api
   ```

4. Explore documentation:
   - Swagger UI: `https://localhost:7219/swagger`
   - Scalar UI: `https://localhost:7219/scalar/v1`

---

## License

This project is licensed under the MIT License.
