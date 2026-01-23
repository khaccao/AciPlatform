# AciPlatform - Clean Architecture .NET 9

Một dự án demo Clean Architecture build bằng .NET 9 với cấu trúc chuẩn enterprise.

## 📁 Cấu trúc dự án

```
AciPlatform/
├── AciPlatform.Api/              # API Layer (ASP.NET Core Web API)
│   ├── Controllers/              # API Controllers
│   ├── Properties/               # Configuration properties
│   ├── Filters/                  # Custom filters
│   ├── Program.cs                # Startup configuration
│   └── appsettings.json          # Settings
│
├── AciPlatform.Application/      # Application Layer (Business Logic)
│   ├── Services/                 # Business services
│   ├── Interfaces/               # Service interfaces
│   ├── DTOs/                     # Data Transfer Objects
│   └── Common/                   # Common utilities
│
├── AciPlatform.Domain/           # Domain Layer (Core business rules)
│   ├── Entities/                 # Domain entities
│   ├── ValueObjects/             # Value objects
│   ├── Interfaces/               # Repository interfaces
│   ├── Exceptions/               # Custom domain exceptions
│   └── Enums/                    # Enumerations
│
├── AciPlatform.Infrastructure/   # Infrastructure Layer (Data access & external services)
│   ├── Persistence/              # DbContext
│   ├── Repositories/             # Repository implementations
│   ├── Migrations/               # EF Core migrations
│   └── ServiceCollectionExtensions.cs
│
└── AciPlatform.sln               # Solution file
```

## 🏗️ Clean Architecture Dependencies

```
AciPlatform.Api
    ↓
AciPlatform.Application
    ↓
AciPlatform.Domain

AciPlatform.Infrastructure ──→ Application
AciPlatform.Infrastructure ──→ Domain
```

**Quan trọng:** Không có reference ngược lại!

## 🛠️ Công nghệ sử dụng

- **.NET 9**
- **Entity Framework Core 9.0.1** - ORM
- **SQL Server** - Database
- **ASP.NET Core** - Web API

## 📦 NuGet Packages

### AciPlatform.Infrastructure
- `Microsoft.EntityFrameworkCore` (9.0.1)
- `Microsoft.EntityFrameworkCore.SqlServer` (9.0.1)
- `Microsoft.EntityFrameworkCore.Tools` (9.0.1)

### AciPlatform.Api
- (Basic ASP.NET Core packages)

Created: 23/01/2026
.NET Version: 9.0
