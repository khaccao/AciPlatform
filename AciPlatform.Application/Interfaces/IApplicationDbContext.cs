using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using AciPlatform.Domain.Entities.LuongPhucLoi;
using AciPlatform.Domain.Entities.HopDong;
using AciPlatform.Domain.Entities.ChamCong;
using AciPlatform.Domain.Entities.Auth;
using AciPlatform.Domain.Entities.MultiChannel;
using AciPlatform.Domain.Entities.QLKho;
using AciPlatform.Domain.Entities.FleetTransportation;
using AciPlatform.Domain.Entities.Sell;
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

    // R&D Project Management
    DbSet<Project> Projects { get; }
    DbSet<ProjectMember> ProjectMembers { get; }
    DbSet<ProjectTask> ProjectTasks { get; }
    DbSet<ProjectDocument> ProjectDocuments { get; }
    
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

    // FleetTransportation
    DbSet<CarFleet> CarFleets { get; }
    DbSet<Car> Cars { get; }
    DbSet<CarField> CarFields { get; }
    DbSet<CarFieldSetup> CarFieldSetups { get; }
    DbSet<CarLocation> CarLocations { get; }
    DbSet<CarLocationDetail> CarLocationDetails { get; }
    DbSet<DriverRouter> DriverRouters { get; }
    DbSet<DriverRouterDetail> DriverRouterDetails { get; }
    DbSet<PetrolConsumption> PetrolConsumptions { get; }
    DbSet<PetrolConsumptionPoliceCheckPoint> PetrolConsumptionPoliceCheckPoints { get; }
    DbSet<PoliceCheckPoint> PoliceCheckPoints { get; }
    DbSet<RoadRoute> RoadRoutes { get; }
    
    // Sell Module
    DbSet<GoodCustomer> GoodCustomers { get; }
    DbSet<GoodDetail> GoodDetails { get; }
    DbSet<Goods> Goods { get; }
    DbSet<GoodsPriceList> GoodsPriceLists { get; }
    DbSet<GoodsPromotion> GoodsPromotions { get; }
    DbSet<GoodsPromotionDetail> GoodsPromotionDetails { get; }
    DbSet<GoodsQuota> GoodsQuotas { get; }
    DbSet<GoodsQuotaDetail> GoodsQuotaDetails { get; }
    DbSet<GoodsQuotaRecipe> GoodsQuotaRecipes { get; }
    DbSet<GoodsQuotaStep> GoodsQuotaSteps { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderDetail> OrderDetails { get; }
    DbSet<OrderSuccessful> OrderSuccessfuls { get; }
    DbSet<Payer> Payers { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

