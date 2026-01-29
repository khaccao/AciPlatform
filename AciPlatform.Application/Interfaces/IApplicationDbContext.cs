using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using AciPlatform.Domain.Entities.LuongPhucLoi;
using AciPlatform.Domain.Entities.HopDong;
using AciPlatform.Domain.Entities.ChamCong;
using AciPlatform.Domain.Entities.Auth;
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
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

