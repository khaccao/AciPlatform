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

## 🚀 Cách chạy

### 1. Restore packages
```bash
dotnet restore
```

### 2. Build solution
```bash
dotnet build
```

### 3. Update database (migrations)
```bash
dotnet ef database update -p AciPlatform.Infrastructure
```

### 4. Run API
```bash
dotnet run --project AciPlatform.Api
```

API sẽ chạy tại: `http://localhost:5041`

## 📚 API Endpoints

### Users
- **GET** `/api/users` - Lấy danh sách tất cả users
- **GET** `/api/users/{id}` - Lấy user theo ID
- **GET** `/api/users/email/{email}` - Tìm user theo email
- **POST** `/api/users` - Tạo user mới
  ```json
  {
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com"
  }
  ```
- **PUT** `/api/users/{id}` - Cập nhật user
  ```json
  {
    "firstName": "Jane",
    "lastName": "Smith"
  }
  ```
- **DELETE** `/api/users/{id}` - Xóa user

## 📋 File nền tảng đã tạo

### Domain
- `Entities/User.cs` - User entity
- `ValueObjects/Email.cs` - Email value object
- `Exceptions/DomainException.cs` - Base domain exception
- `Interfaces/IUserRepository.cs` - Repository interface

### Application
- `Services/UserService.cs` - User business logic
- `Interfaces/IUserService.cs` - Service interface
- `DTOs/UserDto.cs` - Data transfer object

### Infrastructure
- `Persistence/ApplicationDbContext.cs` - EF Core context
- `Repositories/UserRepository.cs` - Repository implementation
- `ServiceCollectionExtensions.cs` - Dependency injection setup
- `ApplicationDbContextFactory.cs` - DbContext factory

### Api
- `Controllers/UsersController.cs` - User API controller

## 🔧 Cấu hình Database

**Default connection string:**
```
Server=(localdb)\mssqllocaldb;Database=AciPlatformDb;Trusted_Connection=true;
```

Có thể thay đổi trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;"
  }
}
```

## 📝 Tạo migration mới

```bash
dotnet ef migrations add {MigrationName} -p AciPlatform.Infrastructure
dotnet ef database update -p AciPlatform.Infrastructure
```

## 🎯 Tiếp theo

- [ ] Thêm Swagger/OpenAPI khi phiên bản tương thích
- [ ] Thêm unit tests (xUnit / NUnit)
- [ ] Thêm validation (FluentValidation)
- [ ] Thêm logging (Serilog)
- [ ] Thêm authentication/authorization (JWT)
- [ ] Thêm caching (Redis)

## 📞 Contact

Created: 23/01/2026
.NET Version: 9.0
