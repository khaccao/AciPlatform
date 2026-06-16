using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using AciPlatform.Domain.Entities.Sell;
using AciPlatform.Domain.Entities.QLKho;
using AciPlatform.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AciPlatform.Infrastructure.Persistence.Seeders;

public static class DatabaseSeeder
{
    private static (byte[] hash, byte[] salt) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        return (hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)), hmac.Key);
    }

    public static async Task SeedAllAsync(IApplicationDbContext context)
    {
        await SeedUserRolesAsync(context);
        await SeedUsersAsync(context);
        await SeedMenusAsync(context);
        await SeedRestaurantErpMenusAsync(context);
        await SeedMenuRolesAsync(context);
        await SeedRestaurantErpMenuRolesAsync(context);
        await SeedSampleDataAsync(context);
    }

    // 1. USER ROLES
    private static async Task SeedUserRolesAsync(IApplicationDbContext context)
    {
        if (await context.UserRoles.AnyAsync()) { Console.WriteLine("â­  UserRoles skipped."); return; }
        context.UserRoles.AddRange(
            new UserRole { Code = "SuperAdmin",     Title = "Super Admin",          Order = 1,  IsNotAllowDelete = true  },
            new UserRole { Code = "Admin",          Title = "Quáº£n trá»‹ viĂªn cĂ´ng ty",Order = 2,  IsNotAllowDelete = true  },
            new UserRole { Code = "HR_Manager",     Title = "Quáº£n lĂ½ NhĂ¢n sá»±",      Order = 3,  IsNotAllowDelete = false },
            new UserRole { Code = "Accountant",     Title = "Káº¿ toĂ¡n",              Order = 4,  IsNotAllowDelete = false },
            new UserRole { Code = "SaleManager",    Title = "Quáº£n lĂ½ BĂ¡n hĂ ng",     Order = 5,  IsNotAllowDelete = false },
            new UserRole { Code = "Salesman",       Title = "NhĂ¢n viĂªn BĂ¡n hĂ ng",   Order = 6,  IsNotAllowDelete = false },
            new UserRole { Code = "WarehouseStaff", Title = "NhĂ¢n viĂªn Kho",        Order = 7,  IsNotAllowDelete = false },
            new UserRole { Code = "FleetManager",   Title = "Quáº£n lĂ½ Xe",           Order = 8,  IsNotAllowDelete = false },
            new UserRole { Code = "ProjectManager", Title = "Quáº£n lĂ½ Dá»± Ă¡n",        Order = 9,  IsNotAllowDelete = false },
            new UserRole { Code = "Employee",       Title = "NhĂ¢n viĂªn",            Order = 10, IsNotAllowDelete = false }
        );
        await context.SaveChangesAsync();
        Console.WriteLine("âœ… Seeded 10 UserRoles");
    }

    // 2. USERS
    private static async Task SeedUsersAsync(IApplicationDbContext context)
    {
        var allRoles = await context.UserRoles.ToDictionaryAsync(r => r.Code, r => r.Id);
        string RoleIds(params string[] codes) =>
            string.Join(",", codes.Where(allRoles.ContainsKey).Select(c => allRoles[c].ToString()));

        // Danh sĂ¡ch users cáº§n cĂ³ trong há»‡ thá»‘ng
        var userDefs = new[]
        {
            // username, fullname, email, phone, password, roleIds
            ("admin",         "Super Administrator",   "admin@aciplatform.vn", "0900000001", "Admin@123",   RoleIds("SuperAdmin")),
            // ACI
            ("nguyen.van.an", "Nguyá»…n VÄƒn An",         "nguyen.van.an@aci.vn", "0900000002", "Password123", RoleIds("Admin","HR_Manager")),
            ("tran.thi.binh", "Tráº§n Thá»‹ BĂ¬nh",         "tran.thi.binh@aci.vn","0900000003", "Password123", RoleIds("Accountant")),
            ("le.van.cuong",  "LĂª VÄƒn CÆ°á»ng",          "le.van.cuong@aci.vn",  "0900000004", "Password123", RoleIds("SaleManager","Salesman")),
            ("do.thi.phuong", "Äá»— Thá»‹ PhÆ°Æ¡ng",         "do.thi.phuong@aci.vn","0900000007", "Password123", RoleIds("ProjectManager")),
            // BHA
            ("bha.admin",     "Admin BHA Solutions",   "admin@bha.vn",         "0900000009", "Password123", RoleIds("Admin")),
            ("pham.thi.dung", "Pháº¡m Thá»‹ Dung",         "pham.thi.dung@bha.vn","0900000005", "Password123", RoleIds("WarehouseStaff")),
            ("hoang.van.em",  "HoĂ ng VÄƒn Em",          "hoang.van.em@bha.vn",  "0900000006", "Password123", RoleIds("FleetManager")),
            ("vu.van.giang",  "VÅ© VÄƒn Giang",          "vu.van.giang@bha.vn",  "0900000008", "Password123", RoleIds("Employee")),
        };

        // Seed tá»«ng user náº¿u chÆ°a tá»“n táº¡i (idempotent)
        int seededCount = 0;
        foreach (var (username, fullname, email, phone, password, roleIds) in userDefs)
        {
            if (!await context.Users.AnyAsync(u => u.Username == username))
            {
                var (hash, salt) = CreatePasswordHash(password);
                context.Users.Add(new User
                {
                    Username = username, FullName = fullname, Email = email, Phone = phone,
                    UserRoleIds = roleIds, PasswordHash = hash, PasswordSalt = salt,
                    Status = 1, IsDeleted = false, TwoFactorEnabled = false,
                    CreatedDate = DateTime.Now, YearCurrent = DateTime.Now.Year,
                });
                seededCount++;
            }
        }
        if (seededCount > 0)
        {
            await context.SaveChangesAsync();
            Console.WriteLine($"âœ… Seeded {seededCount} Users");
        }
        else
        {
            Console.WriteLine("â­  Users skipped (already exist).");
        }

        // UserCompany â€“ seed LUĂ”N LUĂ”N (idempotent, cháº¡y sau khi users Ä‘Ă£ cĂ³)
        var userMap = await context.Users.Where(u => !u.IsDeleted).ToDictionaryAsync(u => u.Username, u => u.Id);
        int Uid(string n) => userMap.GetValueOrDefault(n, 0);

        var assignments = new (int UserId, string CompanyCode)[]
        {
            (Uid("admin"),         "ACI"), (Uid("admin"),         "BHA"), (Uid("admin"),         "HOMEHG"),
            (Uid("nguyen.van.an"), "ACI"), (Uid("tran.thi.binh"), "ACI"),
            (Uid("le.van.cuong"),  "ACI"), (Uid("do.thi.phuong"), "ACI"),
            (Uid("bha.admin"),     "BHA"), (Uid("pham.thi.dung"), "BHA"),
            (Uid("hoang.van.em"),  "BHA"), (Uid("vu.van.giang"),  "BHA"),
        };

        int ucSeeded = 0;
        foreach (var (uid, code) in assignments.Where(a => a.UserId > 0))
        {
            bool exists = await context.UserCompanies.AnyAsync(x => x.UserId == uid && x.CompanyCode == code);
            if (!exists)
            {
                context.UserCompanies.Add(new UserCompany { UserId = uid, CompanyCode = code });
                ucSeeded++;
            }
        }
        if (ucSeeded > 0)
        {
            await context.SaveChangesAsync();
            Console.WriteLine($"âœ… Seeded {ucSeeded} UserCompanies");
        }
        else
        {
            Console.WriteLine("â­  UserCompanies skipped (already exist).");
        }
    }

    // 3. MENUS
    private static async Task SeedMenusAsync(IApplicationDbContext context)
    {
        if (await context.Menus.AnyAsync()) { Console.WriteLine("â­  Menus skipped."); return; }

        var menus = new List<Menu>
        {
            new Menu { Code="dashboard", Name="Tá»•ng quan",    NameEN="Dashboard",       Icon="LayoutDashboard", IsParent=false, Order=1, Url="/dashboard" },
            new Menu { Code="hr",        Name="NhĂ¢n sá»±",      NameEN="Human Resources", Icon="Users",           IsParent=true,  Order=2, Url="/hr" },
            new Menu { Code="accounting",Name="Káº¿ toĂ¡n",      NameEN="Accounting",      Icon="BookOpen",        IsParent=true,  Order=3, Url="/accounting" },
            new Menu { Code="warehouse", Name="Kho hĂ ng",     NameEN="Warehouse",       Icon="Warehouse",       IsParent=true,  Order=4, Url="/warehouse" },
            new Menu { Code="sell",      Name="BĂ¡n hĂ ng",     NameEN="Sales",           Icon="CreditCard",      IsParent=false, Order=5, Url="/sell" },
            new Menu { Code="customer",  Name="KhĂ¡ch hĂ ng",   NameEN="Customers",       Icon="Users",           IsParent=false, Order=6, Url="/customer" },
            new Menu { Code="goods",     Name="HĂ ng hĂ³a",     NameEN="Goods",           Icon="Package",         IsParent=false, Order=7, Url="/goods" },
            new Menu { Code="fleet",     Name="Quáº£n lĂ½ Xe",   NameEN="Fleet",           Icon="Truck",           IsParent=false, Order=8, Url="/fleet" },
            new Menu { Code="projects",  Name="Dá»± Ă¡n R&D",    NameEN="Projects",        Icon="Briefcase",       IsParent=true,  Order=9, Url="/projects" },
            new Menu { Code="dakenh",    Name="Äa kĂªnh",      NameEN="Multi-Channel",   Icon="Share2",          IsParent=true,  Order=10,Url="/dakenh" },
            new Menu { Code="system",    Name="Há»‡ thá»‘ng",     NameEN="System",          Icon="Settings",        IsParent=true,  Order=11,Url="/system" },
            new Menu { Code="menus",     Name="Quáº£n lĂ½ Menu", NameEN="Menu Management", Icon="List",            IsParent=false, Order=12,Url="/system/menus" }
        };

        foreach (var m in menus)
        {
            var existing = await context.Menus.FirstOrDefaultAsync(x => x.Code == m.Code);
            if (existing == null) context.Menus.Add(m);
            else { 
                existing.Icon = m.Icon; 
                existing.Url = m.Url; 
                existing.IsParent = m.IsParent;
                existing.Name = m.Name;
            }
        }
        await context.SaveChangesAsync();

        var subMenus = new List<Menu>
        {
            new Menu { Code="hr/employees",       Name="Danh sĂ¡ch nhĂ¢n viĂªn", NameEN="Employees",      Icon="Users",          CodeParent="hr", IsParent=false, Order=1, Url="/hr/employees" },
            new Menu { Code="hr/organization",    Name="CÆ¡ cáº¥u tá»• chá»©c",      NameEN="Organization",   Icon="GitBranch",      CodeParent="hr", IsParent=false, Order=2, Url="/hr/organization" },
            new Menu { Code="hr/contracts",       Name="Há»£p Ä‘á»“ng lao Ä‘á»™ng",   NameEN="Contracts",      Icon="FileText",       CodeParent="hr", IsParent=false, Order=3, Url="/hr/contracts" },
            new Menu { Code="hr/timekeeping",     Name="Cháº¥m cĂ´ng",            NameEN="Timekeeping",    Icon="Clock",          CodeParent="hr", IsParent=false, Order=4, Url="/hr/timekeeping" },
            new Menu { Code="hr/face-attendance", Name="Äiá»ƒm danh khuĂ´n máº·t", NameEN="Face Attendance",Icon="Camera",         CodeParent="hr", IsParent=false, Order=5, Url="/hr/face-attendance" },
            new Menu { Code="hr/salary",          Name="Báº£ng lÆ°Æ¡ng",           NameEN="Salary",         Icon="Wallet",         CodeParent="hr", IsParent=false, Order=6, Url="/hr/salary" },
            
            new Menu { Code="accounting/general-ledger",    Name="Sá»• cĂ¡i tá»•ng há»£p",   NameEN="General Ledger",    Icon="BookOpen",       CodeParent="accounting",IsParent=false, Order=1, Url="/accounting/general-ledger" },
            new Menu { Code="accounting/chart-of-accounts", Name="Há»‡ thá»‘ng tĂ i khoáº£n", NameEN="Chart of Accounts", Icon="List",           CodeParent="accounting",IsParent=false, Order=2, Url="/accounting/chart-of-accounts" },
            new Menu { Code="accounting/receipt-voucher",   Name="Phiáº¿u thu",          NameEN="Receipt Voucher",   Icon="CreditCard",     CodeParent="accounting",IsParent=false, Order=3, Url="/accounting/receipt-voucher" },
            new Menu { Code="accounting/payment-voucher",   Name="Phiáº¿u chi",          NameEN="Payment Voucher",   Icon="FileText",       CodeParent="accounting",IsParent=false, Order=4, Url="/accounting/payment-voucher" },
            new Menu { Code="accounting/approve-voucher",   Name="Duyá»‡t chá»©ng tá»«",     NameEN="Approve Voucher",   Icon="ClipboardCheck", CodeParent="accounting",IsParent=false, Order=5, Url="/accounting/approve-voucher" },
            new Menu { Code="accounting/warehouse-receipt", Name="Phiáº¿u nháº­p kho",     NameEN="Warehouse Receipt", Icon="PackagePlus",    CodeParent="accounting",IsParent=false, Order=6, Url="/accounting/warehouse-receipt" },
            new Menu { Code="accounting/suppliers",         Name="NhĂ  cung cáº¥p",       NameEN="Suppliers",         Icon="Users",          CodeParent="accounting",IsParent=false, Order=7, Url="/accounting/suppliers" },
            new Menu { Code="accounting/customer-debt",     Name="CĂ´ng ná»£ khĂ¡ch hĂ ng", NameEN="Customer Debt",     Icon="Wallet",         CodeParent="accounting",IsParent=false, Order=8, Url="/accounting/customer-debt" },
            
            new Menu { Code="warehouse/inventory",  Name="Tá»“n kho",    NameEN="Inventory",    Icon="Package",      CodeParent="warehouse",IsParent=false, Order=1, Url="/warehouse/inventory" },
            new Menu { Code="warehouse/locations",  Name="Vá»‹ trĂ­ kho", NameEN="Locations",    Icon="Layers",       CodeParent="warehouse",IsParent=false, Order=2, Url="/warehouse/locations" },
            
            new Menu { Code="projects/list", Name="Danh sĂ¡ch dá»± Ă¡n", NameEN="Project List",Icon="List",     CodeParent="projects",IsParent=false, Order=1, Url="/projects/list" },
            new Menu { Code="my-tasks",      Name="Viá»‡c cá»§a tĂ´i",    NameEN="My Tasks",    Icon="CheckSquare", CodeParent="projects",IsParent=false, Order=2, Url="/projects/my-tasks" },
            new Menu { Code="system/roles",   Name="PhĂ¢n quyá»n",     NameEN="Role Management",   Icon="ShieldCheck", CodeParent="system",IsParent=false, Order=1, Url="/system/roles" },
            new Menu { Code="system/security",Name="Báº£o máº­t nĂ¢ng cao",NameEN="Advanced Security",Icon="Shield",      CodeParent="system",IsParent=false, Order=2, Url="/system/security" }
        };

        foreach (var m in subMenus)
        {
            var existing = await context.Menus.FirstOrDefaultAsync(x => x.Code == m.Code);
            if (existing == null) context.Menus.Add(m);
            else { 
                existing.Icon = m.Icon; 
                existing.Url = m.Url; 
                existing.CodeParent = m.CodeParent;
                existing.Name = m.Name;
            }
        }
        await context.SaveChangesAsync();
        Console.WriteLine("âœ… Seeded 34 Menus");
    }

    // 4. MENU ROLES â€“ PERMISSION MATRIX
    private static async Task SeedRestaurantErpMenusAsync(IApplicationDbContext context)
    {
        var menus = new List<Menu>
        {
            new Menu { Code="restaurant-erp", Name="ERP Nhà hàng", NameEN="Restaurant ERP", Icon="Store", IsParent=true, Order=4, Url="/restaurant-erp" },
            new Menu { Code="restaurant-erp/dashboard",          Name="Dashboard điều hành",   NameEN="Executive Dashboard", Icon="LayoutDashboard", CodeParent="restaurant-erp", IsParent=false, Order=1,  Url="/restaurant-erp/dashboard" },
            new Menu { Code="restaurant-erp/capital",            Name="Vốn đầu tư",            NameEN="Capital",             Icon="Landmark",        CodeParent="restaurant-erp", IsParent=false, Order=2,  Url="/restaurant-erp/capital" },
            new Menu { Code="restaurant-erp/funds",              Name="Quản lý quỹ",           NameEN="Funds",               Icon="Wallet",          CodeParent="restaurant-erp", IsParent=false, Order=3,  Url="/restaurant-erp/funds" },
            new Menu { Code="restaurant-erp/setup-expenses",     Name="Chi phí setup",         NameEN="Setup Expenses",      Icon="Receipt",         CodeParent="restaurant-erp", IsParent=false, Order=4,  Url="/restaurant-erp/setup-expenses" },
            new Menu { Code="restaurant-erp/materials",          Name="Danh mục vật tư",       NameEN="Materials",           Icon="Package",         CodeParent="restaurant-erp", IsParent=false, Order=5,  Url="/restaurant-erp/materials" },
            new Menu { Code="restaurant-erp/purchase-requests",  Name="Đề nghị mua hàng",      NameEN="Purchase Requests",   Icon="FilePlus2",       CodeParent="restaurant-erp", IsParent=false, Order=6,  Url="/restaurant-erp/purchase-requests" },
            new Menu { Code="restaurant-erp/purchase-approvals", Name="Duyệt đề nghị mua",     NameEN="Purchase Approvals",  Icon="ClipboardCheck",  CodeParent="restaurant-erp", IsParent=false, Order=7,  Url="/restaurant-erp/purchase-approvals" },
            new Menu { Code="restaurant-erp/purchase-orders",    Name="Đơn mua hàng",          NameEN="Purchase Orders",     Icon="ShoppingCart",    CodeParent="restaurant-erp", IsParent=false, Order=8,  Url="/restaurant-erp/purchase-orders" },
            new Menu { Code="restaurant-erp/goods-receipts",     Name="Nhập kho",              NameEN="Goods Receipts",      Icon="PackagePlus",     CodeParent="restaurant-erp", IsParent=false, Order=9,  Url="/restaurant-erp/goods-receipts" },
            new Menu { Code="restaurant-erp/payment-requests",   Name="Đề nghị chi",           NameEN="Payment Requests",    Icon="FileText",        CodeParent="restaurant-erp", IsParent=false, Order=10, Url="/restaurant-erp/payment-requests" },
            new Menu { Code="restaurant-erp/payment-approvals",  Name="Duyệt chi",             NameEN="Payment Approvals",   Icon="BadgeCheck",      CodeParent="restaurant-erp", IsParent=false, Order=11, Url="/restaurant-erp/payment-approvals" },
            new Menu { Code="restaurant-erp/disbursements",      Name="Giải ngân",             NameEN="Disbursements",       Icon="Send",            CodeParent="restaurant-erp", IsParent=false, Order=12, Url="/restaurant-erp/disbursements" },
            new Menu { Code="restaurant-erp/supplier-debts",     Name="Công nợ NCC",           NameEN="Supplier Debts",      Icon="CircleDollarSign",CodeParent="restaurant-erp", IsParent=false, Order=13, Url="/restaurant-erp/supplier-debts" },
            new Menu { Code="restaurant-erp/customer-debts",     Name="Công nợ khách hàng",    NameEN="Customer Debts",      Icon="HandCoins",       CodeParent="restaurant-erp", IsParent=false, Order=14, Url="/restaurant-erp/customer-debts" },
            new Menu { Code="restaurant-erp/inventory",          Name="Tồn kho",               NameEN="Inventory",           Icon="Warehouse",       CodeParent="restaurant-erp", IsParent=false, Order=15, Url="/restaurant-erp/inventory" },
        };

        var changed = 0;
        foreach (var menu in menus)
        {
            var existing = await context.Menus.FirstOrDefaultAsync(x => x.Code == menu.Code);
            if (existing == null)
            {
                context.Menus.Add(menu);
                changed++;
            }
            else
            {
                existing.Name = menu.Name;
                existing.NameEN = menu.NameEN;
                existing.Icon = menu.Icon;
                existing.Url = menu.Url;
                existing.CodeParent = menu.CodeParent;
                existing.IsParent = menu.IsParent;
                existing.Order = menu.Order;
            }
        }

        await context.SaveChangesAsync();
        Console.WriteLine(changed > 0 ? $"Seeded {changed} Restaurant ERP menus" : "Restaurant ERP menus checked");
    }

    private static async Task SeedRestaurantErpMenuRolesAsync(IApplicationDbContext context)
    {
        var menus = await context.Menus
            .Where(m => m.Code == "restaurant-erp" || m.Code.StartsWith("restaurant-erp/"))
            .ToListAsync();
        if (!menus.Any()) return;

        var roles = await context.UserRoles.ToDictionaryAsync(r => r.Code, r => r.Id);
        var allowedRoles = new[] { "SuperAdmin", "Admin", "Accountant", "WarehouseStaff" };
        var created = 0;

        foreach (var roleCode in allowedRoles)
        {
            if (!roles.TryGetValue(roleCode, out var roleId)) continue;

            foreach (var menu in menus)
            {
                var exists = await context.MenuRoles.AnyAsync(x => x.MenuId == menu.Id && x.UserRoleId == roleId);
                if (exists) continue;

                var canWarehouseWrite = menu.Code.Contains("goods-receipts") || menu.Code.Contains("inventory");
                context.MenuRoles.Add(new MenuRole
                {
                    MenuId = menu.Id,
                    UserRoleId = roleId,
                    MenuCode = menu.Code,
                    View = true,
                    Add = roleCode != "WarehouseStaff" || canWarehouseWrite,
                    Edit = roleCode != "WarehouseStaff" || canWarehouseWrite,
                    Delete = roleCode is "SuperAdmin" or "Admin",
                    Approve = roleCode is "SuperAdmin" or "Admin" or "Accountant"
                });
                created++;
            }
        }

        if (created > 0)
        {
            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded {created} Restaurant ERP menu permissions");
        }
    }

    private static async Task SeedMenuRolesAsync(IApplicationDbContext context)
    {
        if (await context.MenuRoles.AnyAsync()) { Console.WriteLine("â­  MenuRoles skipped."); return; }

        var allMenus    = await context.Menus.ToListAsync();
        var allMenuIds  = allMenus.Select(m => m.Id).ToList();
        var codeMap     = allMenus.ToDictionary(m => m.Id, m => m.Code);
        var roles       = await context.UserRoles.ToDictionaryAsync(r => r.Code, r => r.Id);

        int RId(string c) => roles.GetValueOrDefault(c, 0);

        // System menus - chá»‰ SuperAdmin Ä‘Æ°á»£c vĂ o
        var systemOnly = new HashSet<string> { "system", "system/roles", "system/security", "menus" };

        List<int> Ids(params string[] codes) => allMenus
            .Where(m => codes.Any(c => m.Code == c || m.Code.StartsWith(c + "/")))
            .Select(m => m.Id).ToList();

        var mr = new List<MenuRole>();

        MenuRole MR(int mid, int rid, bool a, bool e, bool d, bool ap)
        {
            codeMap.TryGetValue(mid, out var code);
            return new MenuRole { MenuId=mid, UserRoleId=rid, MenuCode=code, View=true, Add=a, Edit=e, Delete=d, Approve=ap };
        }

        void Full(int rid, IEnumerable<int> ids)     => ids.ToList().ForEach(id => mr.Add(MR(id, rid, true,  true,  true,  true)));
        void ReadOnly(int rid, IEnumerable<int> ids)  => ids.ToList().ForEach(id => mr.Add(MR(id, rid, false, false, false, false)));
        void ApproveOnly(int rid, IEnumerable<int> ids)=> ids.ToList().ForEach(id => mr.Add(MR(id, rid, false, false, false, true)));

        // SuperAdmin: táº¥t cáº£ 34 menus, full
        Full(RId("SuperAdmin"), allMenuIds);

        // Admin (company level): táº¥t cáº£ TRá»ª system/roles, system/security, menus, system
        //   settings: view only
        var adminFull = allMenuIds
            .Where(id => !systemOnly.Contains(codeMap.GetValueOrDefault(id,"")) && codeMap.GetValueOrDefault(id,"") != "settings")
            .ToList();
        Full(RId("Admin"), adminFull);
        ReadOnly(RId("Admin"), Ids("settings"));

        // HR_Manager: full HR + view dashboard, projects, my-tasks
        Full(RId("HR_Manager"), Ids("hr"));
        ReadOnly(RId("HR_Manager"), Ids("dashboard", "projects", "my-tasks"));

        // Accountant: full accounting + view dashboard, customer, goods
        Full(RId("Accountant"), Ids("accounting"));
        ReadOnly(RId("Accountant"), Ids("dashboard", "customer", "goods"));

        // SaleManager: full sell/customer/goods + approve debt + view dashboard/receipts
        Full(RId("SaleManager"), Ids("sell", "customer", "goods"));
        ApproveOnly(RId("SaleManager"), Ids("accounting", "accounting/customer-debt"));
        ReadOnly(RId("SaleManager"), Ids("dashboard", "accounting/receipt-voucher", "accounting/payment-voucher"));

        // Salesman: sell add+edit (no delete) + view dashboard/customer/goods
        Ids("sell").ForEach(id => mr.Add(MR(id, RId("Salesman"), true, true, false, false)));
        ReadOnly(RId("Salesman"), Ids("dashboard", "customer", "goods"));

        // WarehouseStaff: full warehouse + view dashboard, goods
        Full(RId("WarehouseStaff"), Ids("warehouse"));
        ReadOnly(RId("WarehouseStaff"), Ids("dashboard", "goods"));

        // FleetManager: full fleet + view dashboard
        Full(RId("FleetManager"), Ids("fleet"));
        ReadOnly(RId("FleetManager"), Ids("dashboard"));

        // ProjectManager: full projects + view dashboard, hr/employees
        Full(RId("ProjectManager"), Ids("projects", "my-tasks"));
        ReadOnly(RId("ProjectManager"), Ids("dashboard", "hr/employees"));

        // Employee: view dashboard + my-tasks
        ReadOnly(RId("Employee"), Ids("dashboard", "my-tasks"));

        context.MenuRoles.AddRange(mr);
        await context.SaveChangesAsync();
        Console.WriteLine($"âœ… Seeded {mr.Count} MenuRole entries");
    }

    // 5. SAMPLE DATA
    private static async Task SeedSampleDataAsync(IApplicationDbContext context)
    {
        // Departments
        if (!await context.Departments.AnyAsync())
        {
            context.Departments.AddRange(
                new Department { Code="ACI-IT",  Name="PhĂ²ng CĂ´ng nghá»‡ thĂ´ng tin", CompanyCode="ACI", CreatedDate=DateTime.Now },
                new Department { Code="ACI-HR",  Name="PhĂ²ng NhĂ¢n sá»±",              CompanyCode="ACI", CreatedDate=DateTime.Now },
                new Department { Code="ACI-ACC", Name="PhĂ²ng Káº¿ toĂ¡n",              CompanyCode="ACI", CreatedDate=DateTime.Now },
                new Department { Code="ACI-SL",  Name="PhĂ²ng Kinh doanh",           CompanyCode="ACI", CreatedDate=DateTime.Now },
                new Department { Code="BHA-OPS", Name="PhĂ²ng Váº­n hĂ nh",             CompanyCode="BHA", CreatedDate=DateTime.Now },
                new Department { Code="BHA-WH",  Name="PhĂ²ng Kho váº­n",              CompanyCode="BHA", CreatedDate=DateTime.Now },
                new Department { Code="BHA-FT",  Name="PhĂ²ng Quáº£n lĂ½ Xe",           CompanyCode="BHA", CreatedDate=DateTime.Now }
            );
            await context.SaveChangesAsync();
            Console.WriteLine("âœ… Seeded 7 Departments");
        }

        // Companies stored in Customers table. Hotel companies still live here;
        // their hotel operational data is keyed by the same code in AciPlatform_Hotel.
        var existCodes = await context.Customers
            .Where(c => new[]{"ACI","BHA","HOMEHG"}.Contains(c.Code)).Select(c => c.Code).ToListAsync();

        if (!existCodes.Contains("ACI"))
            context.Customers.Add(new Customer { Code="ACI", Name="CĂ´ng ty Cá»• pháº§n ACI Technology",
                Phone="0281234567", Email="contact@aci.vn", Address="123 Nguyá»…n VÄƒn Linh, Q.7, TP.HCM",
                IsDeleted=false, CreatedDate=DateTime.Now });

        if (!existCodes.Contains("BHA"))
            context.Customers.Add(new Customer { Code="BHA", Name="CĂ´ng ty TNHH BHA Solutions",
                Phone="0281234568", Email="contact@bha.vn", Address="456 LĂª Äáº¡i HĂ nh, Q.11, TP.HCM",
                IsDeleted=false, CreatedDate=DateTime.Now });

        if (!existCodes.Contains("HOMEHG"))
            context.Customers.Add(new Customer { Code="HOMEHG", Name="Home HG - Nha Nghi Ha Giang",
                Phone="", Email="", Address="Ha Giang",
                IsHotel=true, HotelType="HOTEL", IsDeleted=false, CreatedDate=DateTime.Now });
        else
        {
            var homeHg = await context.Customers.FirstOrDefaultAsync(c => c.Code == "HOMEHG");
            if (homeHg != null)
            {
                homeHg.IsHotel = true;
                homeHg.HotelType = "HOTEL";
                homeHg.IsDeleted = false;
                homeHg.UpdatedDate = DateTime.Now;
            }
        }

        await context.SaveChangesAsync();

        // Sample customers
        var existSample = await context.Customers
            .Where(c => new[]{"CUS0001","CUS0002"}.Contains(c.Code)).Select(c => c.Code).ToListAsync();

        if (!existSample.Contains("CUS0001"))
            context.Customers.Add(new Customer { Code="CUS0001", Name="CĂ´ng ty TNHH Minh PhĂ¡t",
                Phone="0912345678", Email="info@minhphat.vn", Address="123 Nguyá»…n Huá»‡, Q.1, TP.HCM",
                IsDeleted=false, CreatedDate=DateTime.Now });

        if (!existSample.Contains("CUS0002"))
            context.Customers.Add(new Customer { Code="CUS0002", Name="Nguyá»…n Thá»‹ Hoa",
                Phone="0987654321", Email="hoa.nguyen@gmail.com", Address="45 LĂª Lá»£i, Q.3, TP.HCM",
                IsDeleted=false, CreatedDate=DateTime.Now });

        await context.SaveChangesAsync();
        Console.WriteLine("âœ… Seeded Companies (ACI, BHA) + 2 sample Customers");

        // Goods
        if (!await context.Goods.AnyAsync())
        {
            Goods G(string vn, string en, string kr, double price) => new Goods
            {
                MenuType="HangHoa", GoodsType="product", PriceList="default",
                Price=price, SalePrice=price, DiscountPrice=0, Status=1, IsDeleted=false,
                Detail1=vn, DetailName1=vn, Detail2="", DetailName2="",
                Detail1English=en, DetailName1English=en,
                Detail1Korean=kr, DetailName1Korean=kr,
                StockUnit="CĂ¡i", Account="156", AccountName="HĂ ng hĂ³a",
                Warehouse="WH-001", WarehouseName="Kho Trung tĂ¢m TP.HCM",
                Position="", Delivery="",
                Image1="", Image2="", Image3="", Image4="", Image5="",
                WebGoodNameVietNam=vn, WebGoodNameEnglish=en, WebGoodNameKorea=kr,
                TitleVietNam=vn, TitleEnglish=en, TitleKorea=kr,
                ContentVietNam="", ContentEnglish="", ContentKorea="",
                CreateAt=DateTime.Now, UserCreated=1,
            };
            context.Goods.AddRange(
                G("Laptop Dell Latitude 5540","Laptop Dell Latitude 5540","Dell ë…¸í¸ë¶ 5540",22000000),
                G("MĂ n hĂ¬nh LG 27 inch 4K","LG 27-inch 4K Monitor","LG 27́¸́¹˜ 4K ëª¨ë‹ˆí„°",8500000)
            );
            await context.SaveChangesAsync();
            Console.WriteLine("âœ… Seeded 2 Goods");
        }

        // Warehouses
        if (!await context.Warehouses.AnyAsync())
        {
            context.Warehouses.AddRange(
                new Warehouse { Code="WH-001", Name="Kho Trung tĂ¢m TP.HCM", ManagerName="Pháº¡m Thá»‹ Dung", CreatedDate=DateTime.Now, UserCreated=1 },
                new Warehouse { Code="WH-002", Name="Kho HĂ  Ná»™i",            ManagerName="VÅ© VÄƒn Giang",  CreatedDate=DateTime.Now, UserCreated=1 }
            );
            await context.SaveChangesAsync();
            Console.WriteLine("âœ… Seeded 2 Warehouses");
        }

        // Projects
        if (!await context.Projects.AnyAsync())
        {
            var adminId = (await context.Users.FirstOrDefaultAsync(u => u.Username == "admin"))?.Id ?? 1;
            context.Projects.AddRange(
                new Project { Code="PRJ-2025-001", Name="ACI Platform v2 - NĂ¢ng cáº¥p toĂ n diá»‡n",
                    Description="NĂ¢ng cáº¥p háº¡ táº§ng, UI/UX vĂ  tĂ­ch há»£p AI",
                    Status="active", StartDate=new DateTime(2025,1,1), EndDate=new DateTime(2025,12,31),
                    Budget=500000000, CompanyCode="ACI", CreatedAt=DateTime.Now, CreatedBy=adminId },
                new Project { Code="PRJ-2025-002", Name="TĂ­ch há»£p AI & Tá»± Ä‘á»™ng hoĂ¡ quy trĂ¬nh",
                    Description="NghiĂªn cá»©u vĂ  tĂ­ch há»£p AI vĂ o cĂ¡c nghiá»‡p vá»¥ káº¿ toĂ¡n, bĂ¡n hĂ ng",
                    Status="Planned", StartDate=new DateTime(2025,6,1), EndDate=new DateTime(2026,5,31),
                    Budget=300000000, CompanyCode="BHA", CreatedAt=DateTime.Now, CreatedBy=adminId }
            );
            await context.SaveChangesAsync();
            Console.WriteLine("âœ… Seeded 2 Projects");
        }

        // ChartOfAccounts
        var year = DateTime.Now.Year;
        if (!await context.ChartOfAccounts.AnyAsync(c => c.Year == year))
            await ChartOfAccountSeeder.SeedAsync(context, year);

        Console.WriteLine("đŸ‰ DatabaseSeeder completed!");
    }
}

