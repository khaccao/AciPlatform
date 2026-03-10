using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using AciPlatform.Domain.Entities.LuongPhucLoi;
using AciPlatform.Domain.Entities.HopDong;
using AciPlatform.Domain.Entities.ChamCong;
using AciPlatform.Domain.Entities.Auth;
using AciPlatform.Domain.Entities.MultiChannel;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<Menu> Menus { get; }
    DbSet<MenuRole> MenuRoles { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Department> Departments { get; }
    DbSet<PositionDetail> PositionDetails { get; }
    DbSet<Degree> Degrees { get; }
    DbSet<Certificate> Certificates { get; }
    DbSet<Major> Majors { get; }
    DbSet<Relative> Relatives { get; }
    DbSet<HistoryAchievement> HistoryAchievements { get; }
    DbSet<DecisionType> DecisionTypes { get; }
    DbSet<Decide> Decides { get; }
    DbSet<Allowance> Allowances { get; }
    DbSet<AllowanceUser> AllowanceUsers { get; }
    DbSet<SalaryType> SalaryTypes { get; }
    DbSet<ContractType> ContractTypes { get; }
    DbSet<ContractFile> ContractFiles { get; }
    DbSet<UserContractHistory> UserContractHistories { get; }
    DbSet<TimeKeepingEntry> TimeKeepingEntries { get; }
    DbSet<UserCompany> UserCompanies { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<UserMenu> UserMenus { get; }
    
    // MultiChannel
    DbSet<FacebookAppConfig> FacebookAppConfigs { get; }
    DbSet<FacebookPage> FacebookPages { get; }
    DbSet<SocialPost> SocialPosts { get; }
    DbSet<AutomationWorkflow> AutomationWorkflows { get; }
    DbSet<AutomationLog> AutomationLogs { get; }
    
    // QLKho
    DbSet<Warehouse> Warehouses { get; }
    DbSet<WareHouseFloor> WareHouseFloors { get; }
    DbSet<WareHouseShelves> WareHouseShelves { get; }
    DbSet<WareHousePosition> WareHousePositions { get; }
    DbSet<Inventory> Inventories { get; }
    DbSet<WareHouseWithShelves> WareHouseWithShelves { get; }
    DbSet<WareHouseShelvesWithFloors> WareHouseShelvesWithFloors { get; }
    DbSet<WareHouseFloorWithPosition> WareHouseFloorWithPositions { get; }
    DbSet<GoodWarehouses> GoodWarehouses { get; }
    DbSet<GoodWarehousesPositions> GoodWarehousesPositions { get; }
    DbSet<GoodWarehouseExport> GoodWarehouseExports { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

