# LilacTechSys – Backend REST API (.NET 9 & PostgreSQL)

> **Enterprise-Grade Clean Architecture Backend for LilacTechSys Web Platform**  
> Companion Frontend: [https://github.com/vs02354-cloud/LilacWeb.git](https://github.com/vs02354-cloud/LilacWeb.git)

---

## 🏛️ Architecture Overview

The solution adheres strictly to **Clean Architecture** principles, enforcing separation of concerns, testability, and high maintainability:

```
backend/
├── LilacTechSys.sln
├── src/
│   ├── LilacTechSys.Domain/          # Core Domain Entities, Enums, Base Entities (No external dependencies)
│   ├── LilacTechSys.Application/     # CQRS/Service Interfaces, DTOs, FluentValidation rules, Common Results
│   ├── LilacTechSys.Infrastructure/  # EF Core DbContext, Npgsql PostgreSQL Provider, Repositories, File Storage, Auth Helpers
│   └── LilacTechSys.Api/             # ASP.NET Core 9 Web API Controllers, JWT Middleware, Rate Limiting, Swagger OpenAPI
└── tests/
    └── LilacTechSys.Tests/           # xUnit Unit & Integration Tests (Validators, JWT Token Generation)
```

---

## 🚀 Tech Stack

- **Runtime**: [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Framework**: ASP.NET Core Web API
- **ORM**: Entity Framework Core 9.0 with [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.npgsql.org/efcore/)
- **Database**: PostgreSQL 16
- **Authentication**: JWT Bearer Tokens with claims, role-based authorization (`Admin`, `SuperAdmin`), and refresh token rotation
- **Password Hashing**: [BCrypt.Net-Next](https://github.com/BcryptNet/bcrypt.net)
- **Validation**: [FluentValidation](https://fluentvalidation.net/)
- **Logging**: [Serilog](https://serilog.net/) with structured console and file outputs
- **API Documentation**: [Swagger / OpenAPI](https://swagger.io/) with JWT Bearer security definitions
- **Testing**: xUnit with FluentValidation test helpers

---

## 📡 REST API Endpoints Overview

All endpoints are versioned under `/api/v1/`:

| Controller | Method | Route | Description | Auth Required |
|---|---|---|---|---|
| **Health** | `GET` | `/api/v1/health` | Service health status check | Public |
| **Auth** | `POST` | `/api/v1/auth/login` | Admin user authentication & JWT issuance | Public |
| **Auth** | `POST` | `/api/v1/auth/refresh` | Refresh expired access token | Public |
| **Auth** | `GET` | `/api/v1/auth/me` | Current authenticated admin profile | Bearer Token |
| **Services** | `GET` | `/api/v1/services` | Retrieve all 8 practice areas | Public |
| **Services** | `GET` | `/api/v1/services/{slugOrId}` | Retrieve practice detail specifications | Public |
| **Services** | `POST` / `PUT` / `DELETE` | `/api/v1/services` | Manage services (CRUD) | Admin |
| **Projects** | `GET` | `/api/v1/projects` | Filterable portfolio case studies | Public |
| **Projects** | `GET` | `/api/v1/projects/{slugOrId}` | Case study detail with metrics & architecture | Public |
| **Projects** | `POST` / `PUT` / `DELETE` | `/api/v1/projects` | Case study management (CRUD) | Admin |
| **Blog** | `GET` | `/api/v1/blog` | Searchable & paginated technical articles | Public |
| **Blog** | `GET` | `/api/v1/blog/{slugOrId}` | Full article reader payload | Public |
| **Blog** | `POST` / `PUT` / `DELETE` | `/api/v1/blog` | Blog management & category assigner | Admin |
| **Careers** | `GET` | `/api/v1/careers` | Active job openings listing | Public |
| **Careers** | `POST` | `/api/v1/careers/{id}/apply` | Submit job application (multipart PDF upload) | Public |
| **Careers** | `GET` | `/api/v1/careers/applications` | Review candidate pipeline | Admin |
| **Careers** | `PATCH`| `/api/v1/careers/applications/{id}/status` | Update candidate status (0-5) | Admin |
| **Careers** | `GET` | `/api/v1/careers/applications/resume` | Download applicant PDF resume | Admin |
| **Quotes** | `POST` | `/api/v1/quote` | Submit 4-step interactive quote estimate | Public |
| **Quotes** | `GET` | `/api/v1/quote` | Inbound RFQ listing | Admin |
| **Quotes** | `PATCH`| `/api/v1/quote/{id}/status` | Transition quote status (0-4) | Admin |
| **Contact** | `POST` | `/api/v1/contact` | Submit general contact message | Public |
| **Contact** | `GET` | `/api/v1/contact` | Inbox listing | Admin |
| **Contact** | `PATCH`| `/api/v1/contact/{id}/read` | Mark message as read | Admin |
| **Newsletter** | `POST` | `/api/v1/newsletter/subscribe` | Register newsletter subscription | Public |
| **Newsletter** | `GET` | `/api/v1/newsletter/subscribers` | List active subscribers | Admin |
| **Dashboard** | `GET` | `/api/v1/dashboard/stats` | Real-time telemetry counters | Admin |

---

## ⚡ Getting Started

### 1. Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/) running on `localhost:5432`

### 2. Database Setup

#### Option A: Automatic via EF Core (Code-First)
The API automatically creates and seeds the PostgreSQL database on first launch:
```sql
CREATE DATABASE lilactechsys;
```

#### Option B: Standalone SQL Script (Database-First)
You can directly run the provided PostgreSQL schema and seed script located in `database/lilactechsys_database.sql`:
```bash
# Using PowerShell automation script:
powershell -ExecutionPolicy Bypass -File database/init_db.ps1

# Or manually via psql CLI:
psql -h 127.0.0.1 -p 5432 -U postgres -d lilactechsys -f database/lilactechsys_database.sql
```

Update `src/LilacTechSys.Api/appsettings.json` if your PostgreSQL username or password differs from default `postgres/postgres`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=127.0.0.1;Port=5432;Database=lilactechsys;Username=postgres;Password=your_password;Include Error Detail=true"
}
```

### 3. Restore & Build
```bash
# From the backend/ directory
dotnet restore
dotnet build
```

### 4. Run Automated Tests
```bash
dotnet test
```

### 5. Launch API Server
```bash
dotnet run --project src/LilacTechSys.Api/LilacTechSys.Api.csproj
```
- API will start listening on: `http://localhost:5000`
- Swagger UI available at: `http://localhost:5000/swagger`
- The database will automatically initialize schema and populate realistic seed data (8 practice areas, 4 case studies, 3 technical articles, 4 team members, 3 job openings, and default admin user).

---

## ☁️ Deployment on Render

Render does not offer a native .NET 9 runtime, so deployment uses the included production-ready `Dockerfile` and `render.yaml`.

### Render Service Settings:
- **Environment / Runtime**: `Docker`
- **Root Directory**: `.` (or leave empty if repo is just the backend)
- **Dockerfile Path**: `./Dockerfile` (or `Dockerfile`)
- **Docker Build Context**: `.`

### Environment Variables on Render:
| Variable | Value / Description |
|---|---|
| `PORT` | `10000` (Render defaults to this) |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `DATABASE_URL` | Your PostgreSQL connection string or Render Postgres internal URL (e.g. `postgresql://user:password@hostname:5432/dbname`) |
| `JWT_SECRET` | 32+ character random string |

---

## 🔐 Default Admin Account

- **Username**: `admin`
- **Password**: `Admin@123`
- **Role**: `SuperAdmin`

---

## 📄 License
Copyright © 2026 LilacTechSys. All rights reserved.

