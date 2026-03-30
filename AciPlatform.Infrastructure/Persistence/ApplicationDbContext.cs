using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using AciPlatform.Domain.Entities.LuongPhucLoi;
using AciPlatform.Domain.Entities.HopDong;
using AciPlatform.Domain.Entities.ChamCong;
using AciPlatform.Domain.Entities.Auth;
using AciPlatform.Domain.Entities.MultiChannel;
using AciPlatform.Domain.Entities.QLKho;
using AciPlatform.Domain.Entities.FleetTransportation;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuRole> MenuRoles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<PositionDetail> PositionDetails { get; set; }
    public DbSet<Degree> Degrees { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Major> Majors { get; set; }
    public DbSet<Relative> Relatives { get; set; }
    public DbSet<HistoryAchievement> HistoryAchievements { get; set; }
    public DbSet<DecisionType> DecisionTypes { get; set; }
    public DbSet<Decide> Decides { get; set; }
    public DbSet<Allowance> Allowances { get; set; }
    public DbSet<AllowanceUser> AllowanceUsers { get; set; }
    public DbSet<SalaryType> SalaryTypes { get; set; }
    public DbSet<ContractType> ContractTypes { get; set; }
    public DbSet<ContractFile> ContractFiles { get; set; }
    public DbSet<UserContractHistory> UserContractHistories { get; set; }
    public DbSet<TimeKeepingEntry> TimeKeepingEntries { get; set; }
    public DbSet<UserCompany> UserCompanies { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserMenu> UserMenus { get; set; }

    public DbSet<FacebookAppConfig> FacebookAppConfigs { get; set; }
    public DbSet<FacebookPage> FacebookPages { get; set; }
    public DbSet<SocialPost> SocialPosts { get; set; }
    public DbSet<AutomationWorkflow> AutomationWorkflows { get; set; }
    public DbSet<AutomationLog> AutomationLogs { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<WareHouseFloor> WareHouseFloors { get; set; }
    public DbSet<WareHouseShelves> WareHouseShelves { get; set; }
    public DbSet<WareHousePosition> WareHousePositions { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<WareHouseWithShelves> WareHouseWithShelves { get; set; }
    public DbSet<WareHouseShelvesWithFloors> WareHouseShelvesWithFloors { get; set; }
    public DbSet<WareHouseFloorWithPosition> WareHouseFloorWithPositions { get; set; }
    public DbSet<GoodWarehouses> GoodWarehouses { get; set; }
    public DbSet<GoodWarehousesPositions> GoodWarehousesPositions { get; set; }
    public DbSet<GoodWarehouseExport> GoodWarehouseExports { get; set; }

    public DbSet<CarFleet> CarFleets { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarField> CarFields { get; set; }
    public DbSet<CarFieldSetup> CarFieldSetups { get; set; }
    public DbSet<CarLocation> CarLocations { get; set; }
    public DbSet<CarLocationDetail> CarLocationDetails { get; set; }
    public DbSet<DriverRouter> DriverRouters { get; set; }
    public DbSet<DriverRouterDetail> DriverRouterDetails { get; set; }
    public DbSet<PetrolConsumption> PetrolConsumptions { get; set; }
    public DbSet<PetrolConsumptionPoliceCheckPoint> PetrolConsumptionPoliceCheckPoints { get; set; }
    public DbSet<PoliceCheckPoint> PoliceCheckPoints { get; set; }
    public DbSet<RoadRoute> RoadRoutes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // Configure UserRole entity
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Configure Menu entity
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            
            // Self referencing relationship for Parent Menu
            entity.HasOne<Menu>()
                  .WithMany()
                  .HasForeignKey(d => d.CodeParent)
                  .HasPrincipalKey(p => p.Code)
                  .OnDelete(DeleteBehavior.Restrict); 

            // Note: Since CodeParent is string and not Id, we use HasPrincipalKey on Code. 
            // In the sample/db rules, CodeParent FK points to Menu.Code
        });

        // Configure MenuRole entity
        modelBuilder.Entity<MenuRole>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.Menu)
                  .WithMany()
                  .HasForeignKey(d => d.MenuId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.UserRole)
                  .WithMany()
                  .HasForeignKey(d => d.UserRoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Customer entity
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Phone).IsUnique();
        });

        // Configure UserCompany entity
        modelBuilder.Entity<UserCompany>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.CompanyCode }).IsUnique();
            entity.Property(e => e.CompanyCode).HasMaxLength(50);
        });

        // Configure RefreshToken entity
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).HasMaxLength(200);
            entity.HasIndex(e => e.Token).IsUnique();
        });
    }
}
