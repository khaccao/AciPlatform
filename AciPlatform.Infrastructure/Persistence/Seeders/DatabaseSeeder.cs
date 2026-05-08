using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using AciPlatform.Domain.Entities.Sell;
using AciPlatform.Domain.Entities.QLKho;
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

    public static async Task SeedAllAsync(ApplicationDbContext context)
    {
        await SeedUserRolesAsync(context);
        await SeedUsersAsync(context);
        await SeedMenusAsync(context);
        await SeedMenuRolesAsync(context);
        await SeedSampleDataAsync(context);
    }

    // 1. USER ROLES
    private static async Task SeedUserRolesAsync(ApplicationDbContext context)
    {
        if (await context.UserRoles.AnyAsync()) { Console.WriteLine("⏭  UserRoles skipped."); return; }
        context.UserRoles.AddRange(
            new UserRole { Code = "SuperAdmin",     Title = "Super Admin",          Order = 1,  IsNotAllowDelete = true  },
            new UserRole { Code = "Admin",          Title = "Quản trị viên công ty",Order = 2,  IsNotAllowDelete = true  },
            new UserRole { Code = "HR_Manager",     Title = "Quản lý Nhân sự",      Order = 3,  IsNotAllowDelete = false },
            new UserRole { Code = "Accountant",     Title = "Kế toán",              Order = 4,  IsNotAllowDelete = false },
            new UserRole { Code = "SaleManager",    Title = "Quản lý Bán hàng",     Order = 5,  IsNotAllowDelete = false },
            new UserRole { Code = "Salesman",       Title = "Nhân viên Bán hàng",   Order = 6,  IsNotAllowDelete = false },
            new UserRole { Code = "WarehouseStaff", Title = "Nhân viên Kho",        Order = 7,  IsNotAllowDelete = false },
            new UserRole { Code = "FleetManager",   Title = "Quản lý Xe",           Order = 8,  IsNotAllowDelete = false },
            new UserRole { Code = "ProjectManager", Title = "Quản lý Dự án",        Order = 9,  IsNotAllowDelete = false },
            new UserRole { Code = "Employee",       Title = "Nhân viên",            Order = 10, IsNotAllowDelete = false }
        );
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Seeded 10 UserRoles");
    }

    // 2. USERS
    private static async Task SeedUsersAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync()) { Console.WriteLine("⏭  Users skipped."); return; }

        var allRoles = await context.UserRoles.ToDictionaryAsync(r => r.Code, r => r.Id);
        string RoleIds(params string[] codes) =>
            string.Join(",", codes.Where(allRoles.ContainsKey).Select(c => allRoles[c].ToString()));

        var users = new[]
        {
            // username, fullname, email, phone, password, roleIds
            ("admin",         "Super Administrator",   "admin@aciplatform.vn", "0900000001", "Admin@123",   RoleIds("SuperAdmin")),
            // ACI
            ("nguyen.van.an", "Nguyễn Văn An",         "nguyen.van.an@aci.vn", "0900000002", "Password123", RoleIds("Admin","HR_Manager")),
            ("tran.thi.binh", "Trần Thị Bình",         "tran.thi.binh@aci.vn","0900000003", "Password123", RoleIds("Accountant")),
            ("le.van.cuong",  "Lê Văn Cường",          "le.van.cuong@aci.vn",  "0900000004", "Password123", RoleIds("SaleManager","Salesman")),
            ("do.thi.phuong", "Đỗ Thị Phương",         "do.thi.phuong@aci.vn","0900000007", "Password123", RoleIds("ProjectManager")),
            // BHA
            ("bha.admin",     "Admin BHA Solutions",   "admin@bha.vn",         "0900000009", "Password123", RoleIds("Admin")),
            ("pham.thi.dung", "Phạm Thị Dung",         "pham.thi.dung@bha.vn","0900000005", "Password123", RoleIds("WarehouseStaff")),
            ("hoang.van.em",  "Hoàng Văn Em",          "hoang.van.em@bha.vn",  "0900000006", "Password123", RoleIds("FleetManager")),
            ("vu.van.giang",  "Vũ Văn Giang",          "vu.van.giang@bha.vn",  "0900000008", "Password123", RoleIds("Employee")),
        };

        foreach (var (username, fullname, email, phone, password, roleIds) in users)
        {
            var (hash, salt) = CreatePasswordHash(password);
            context.Users.Add(new User
            {
                Username = username, FullName = fullname, Email = email, Phone = phone,
                UserRoleIds = roleIds, PasswordHash = hash, PasswordSalt = salt,
                Status = 1, IsDeleted = false, TwoFactorEnabled = false,
                CreatedDate = DateTime.Now, YearCurrent = DateTime.Now.Year,
            });
        }
        await context.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {users.Length} Users");

        // UserCompany
        if (!await context.UserCompanies.AnyAsync())
        {
            var map = await context.Users.ToDictionaryAsync(u => u.Username, u => u.Id);
            int Uid(string n) => map.GetValueOrDefault(n, 0);
            var asgn = new (int, string)[]
            {
                (Uid("admin"),         "ACI"), (Uid("admin"),         "BHA"),
                (Uid("nguyen.van.an"), "ACI"), (Uid("tran.thi.binh"), "ACI"),
                (Uid("le.van.cuong"),  "ACI"), (Uid("do.thi.phuong"), "ACI"),
                (Uid("bha.admin"),     "BHA"), (Uid("pham.thi.dung"), "BHA"),
                (Uid("hoang.van.em"),  "BHA"), (Uid("vu.van.giang"),  "BHA"),
            };
            foreach (var (uid, code) in asgn.Where(a => a.Item1 > 0))
                context.UserCompanies.Add(new UserCompany { UserId = uid, CompanyCode = code });
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ Seeded {asgn.Length} UserCompanies");
        }
    }

    // 3. MENUS
    private static async Task SeedMenusAsync(ApplicationDbContext context)
    {
        if (await context.Menus.AnyAsync()) { Console.WriteLine("⏭  Menus skipped."); return; }

        context.Menus.AddRange(
            new Menu { Code="dashboard", Name="Tổng quan",    NameEN="Dashboard",       Icon="dashboard",       IsParent=false, Order=1  },
            new Menu { Code="hr",        Name="Nhân sự",      NameEN="Human Resources", Icon="people",          IsParent=true,  Order=2  },
            new Menu { Code="accounting",Name="Kế toán",      NameEN="Accounting",      Icon="account_balance", IsParent=true,  Order=3  },
            new Menu { Code="warehouse", Name="Kho hàng",     NameEN="Warehouse",       Icon="warehouse",       IsParent=true,  Order=4  },
            new Menu { Code="sell",      Name="Bán hàng",     NameEN="Sales",           Icon="point_of_sale",   IsParent=false, Order=5  },
            new Menu { Code="customer",  Name="Khách hàng",   NameEN="Customers",       Icon="person_pin",      IsParent=false, Order=6  },
            new Menu { Code="goods",     Name="Hàng hóa",     NameEN="Goods",           Icon="inventory_2",     IsParent=false, Order=7  },
            new Menu { Code="fleet",     Name="Quản lý Xe",   NameEN="Fleet",           Icon="local_shipping",  IsParent=false, Order=8  },
            new Menu { Code="projects",  Name="Dự án R&D",    NameEN="Projects",        Icon="folder_special",  IsParent=true,  Order=9  },
            new Menu { Code="dakenh",    Name="Đa kênh",      NameEN="Multi-Channel",   Icon="hub",             IsParent=true,  Order=10 },
            new Menu { Code="system",    Name="Hệ thống",     NameEN="System",          Icon="settings",        IsParent=true,  Order=11 },
            new Menu { Code="menus",     Name="Quản lý Menu", NameEN="Menu Management", Icon="menu_book",       IsParent=false, Order=12 }
        );
        await context.SaveChangesAsync();

        context.Menus.AddRange(
            new Menu { Code="hr/employees",       Name="Danh sách nhân viên", NameEN="Employees",      Icon="badge",                  CodeParent="hr",        IsParent=false, Order=1 },
            new Menu { Code="hr/organization",    Name="Cơ cấu tổ chức",      NameEN="Organization",   Icon="account_tree",           CodeParent="hr",        IsParent=false, Order=2 },
            new Menu { Code="hr/contracts",       Name="Hợp đồng lao động",   NameEN="Contracts",      Icon="description",            CodeParent="hr",        IsParent=false, Order=3 },
            new Menu { Code="hr/timekeeping",     Name="Chấm công",            NameEN="Timekeeping",    Icon="schedule",               CodeParent="hr",        IsParent=false, Order=4 },
            new Menu { Code="hr/face-attendance", Name="Điểm danh khuôn mặt", NameEN="Face Attendance",Icon="face",                   CodeParent="hr",        IsParent=false, Order=5 },
            new Menu { Code="hr/salary",          Name="Bảng lương",           NameEN="Salary",         Icon="payments",               CodeParent="hr",        IsParent=false, Order=6 },
            new Menu { Code="accounting/general-ledger",    Name="Sổ cái tổng hợp",   NameEN="General Ledger",    Icon="book",                   CodeParent="accounting",IsParent=false, Order=1 },
            new Menu { Code="accounting/chart-of-accounts", Name="Hệ thống tài khoản", NameEN="Chart of Accounts", Icon="format_list_numbered",   CodeParent="accounting",IsParent=false, Order=2 },
            new Menu { Code="accounting/receipt-voucher",   Name="Phiếu thu",          NameEN="Receipt Voucher",   Icon="receipt",                CodeParent="accounting",IsParent=false, Order=3 },
            new Menu { Code="accounting/payment-voucher",   Name="Phiếu chi",          NameEN="Payment Voucher",   Icon="money_off",              CodeParent="accounting",IsParent=false, Order=4 },
            new Menu { Code="accounting/approve-voucher",   Name="Duyệt chứng từ",     NameEN="Approve Voucher",   Icon="approval",               CodeParent="accounting",IsParent=false, Order=5 },
            new Menu { Code="accounting/warehouse-receipt", Name="Phiếu nhập kho",     NameEN="Warehouse Receipt", Icon="input",                  CodeParent="accounting",IsParent=false, Order=6 },
            new Menu { Code="accounting/suppliers",         Name="Nhà cung cấp",       NameEN="Suppliers",         Icon="business",               CodeParent="accounting",IsParent=false, Order=7 },
            new Menu { Code="accounting/customer-debt",     Name="Công nợ khách hàng", NameEN="Customer Debt",     Icon="account_balance_wallet", CodeParent="accounting",IsParent=false, Order=8 },
            new Menu { Code="warehouse/inventory",  Name="Tồn kho",    NameEN="Inventory",    Icon="inventory",            CodeParent="warehouse",IsParent=false, Order=1 },
            new Menu { Code="warehouse/locations",  Name="Vị trí kho", NameEN="Locations",    Icon="location_on",          CodeParent="warehouse",IsParent=false, Order=2 },
            new Menu { Code="projects/list", Name="Danh sách dự án", NameEN="Project List",Icon="view_list", CodeParent="projects",IsParent=false, Order=1 },
            new Menu { Code="my-tasks",      Name="Việc của tôi",    NameEN="My Tasks",    Icon="task_alt",  CodeParent="projects",IsParent=false, Order=2 },
            new Menu { Code="dakenh/facebook",Name="Facebook",       NameEN="Facebook",    Icon="facebook",  CodeParent="dakenh", IsParent=false, Order=1 },
            new Menu { Code="system/roles",   Name="Phân quyền",     NameEN="Role Management",   Icon="admin_panel_settings",CodeParent="system",IsParent=false, Order=1 },
            new Menu { Code="system/security",Name="Bảo mật nâng cao",NameEN="Advanced Security",Icon="security",           CodeParent="system",IsParent=false, Order=2 },
            new Menu { Code="settings",       Name="Cài đặt",        NameEN="Settings",          Icon="tune",               CodeParent="system",IsParent=false, Order=3 }
        );
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Seeded 34 Menus");
    }

    // 4. MENU ROLES – PERMISSION MATRIX
    private static async Task SeedMenuRolesAsync(ApplicationDbContext context)
    {
        if (await context.MenuRoles.AnyAsync()) { Console.WriteLine("⏭  MenuRoles skipped."); return; }

        var allMenus    = await context.Menus.ToListAsync();
        var allMenuIds  = allMenus.Select(m => m.Id).ToList();
        var codeMap     = allMenus.ToDictionary(m => m.Id, m => m.Code);
        var roles       = await context.UserRoles.ToDictionaryAsync(r => r.Code, r => r.Id);

        int RId(string c) => roles.GetValueOrDefault(c, 0);

        // System menus - chỉ SuperAdmin được vào
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

        // SuperAdmin: tất cả 34 menus, full
        Full(RId("SuperAdmin"), allMenuIds);

        // Admin (company level): tất cả TRỪ system/roles, system/security, menus, system
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
        Console.WriteLine($"✅ Seeded {mr.Count} MenuRole entries");
    }

    // 5. SAMPLE DATA
    private static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Departments
        if (!await context.Departments.AnyAsync())
        {
            context.Departments.AddRange(
                new Department { Code="ACI-IT",  Name="Phòng Công nghệ thông tin", CompanyCode="ACI", CreatedDate=DateTime.Now },
                new Department { Code="ACI-HR",  Name="Phòng Nhân sự",              CompanyCode="ACI", CreatedDate=DateTime.Now },
                new Department { Code="ACI-ACC", Name="Phòng Kế toán",              CompanyCode="ACI", CreatedDate=DateTime.Now },
                new Department { Code="ACI-SL",  Name="Phòng Kinh doanh",           CompanyCode="ACI", CreatedDate=DateTime.Now },
                new Department { Code="BHA-OPS", Name="Phòng Vận hành",             CompanyCode="BHA", CreatedDate=DateTime.Now },
                new Department { Code="BHA-WH",  Name="Phòng Kho vận",              CompanyCode="BHA", CreatedDate=DateTime.Now },
                new Department { Code="BHA-FT",  Name="Phòng Quản lý Xe",           CompanyCode="BHA", CreatedDate=DateTime.Now }
            );
            await context.SaveChangesAsync();
            Console.WriteLine("✅ Seeded 7 Departments");
        }

        // Companies (ACI, BHA) stored in Customers table
        var existCodes = await context.Customers
            .Where(c => new[]{"ACI","BHA"}.Contains(c.Code)).Select(c => c.Code).ToListAsync();

        if (!existCodes.Contains("ACI"))
            context.Customers.Add(new Customer { Code="ACI", Name="Công ty Cổ phần ACI Technology",
                Phone="0281234567", Email="contact@aci.vn", Address="123 Nguyễn Văn Linh, Q.7, TP.HCM",
                IsDeleted=false, CreatedDate=DateTime.Now });

        if (!existCodes.Contains("BHA"))
            context.Customers.Add(new Customer { Code="BHA", Name="Công ty TNHH BHA Solutions",
                Phone="0281234568", Email="contact@bha.vn", Address="456 Lê Đại Hành, Q.11, TP.HCM",
                IsDeleted=false, CreatedDate=DateTime.Now });

        await context.SaveChangesAsync();

        // Sample customers
        var existSample = await context.Customers
            .Where(c => new[]{"CUS0001","CUS0002"}.Contains(c.Code)).Select(c => c.Code).ToListAsync();

        if (!existSample.Contains("CUS0001"))
            context.Customers.Add(new Customer { Code="CUS0001", Name="Công ty TNHH Minh Phát",
                Phone="0912345678", Email="info@minhphat.vn", Address="123 Nguyễn Huệ, Q.1, TP.HCM",
                IsDeleted=false, CreatedDate=DateTime.Now });

        if (!existSample.Contains("CUS0002"))
            context.Customers.Add(new Customer { Code="CUS0002", Name="Nguyễn Thị Hoa",
                Phone="0987654321", Email="hoa.nguyen@gmail.com", Address="45 Lê Lợi, Q.3, TP.HCM",
                IsDeleted=false, CreatedDate=DateTime.Now });

        await context.SaveChangesAsync();
        Console.WriteLine("✅ Seeded Companies (ACI, BHA) + 2 sample Customers");

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
                StockUnit="Cái", Account="156", AccountName="Hàng hóa",
                Warehouse="WH-001", WarehouseName="Kho Trung tâm TP.HCM",
                Position="", Delivery="",
                Image1="", Image2="", Image3="", Image4="", Image5="",
                WebGoodNameVietNam=vn, WebGoodNameEnglish=en, WebGoodNameKorea=kr,
                TitleVietNam=vn, TitleEnglish=en, TitleKorea=kr,
                ContentVietNam="", ContentEnglish="", ContentKorea="",
                CreateAt=DateTime.Now, UserCreated=1,
            };
            context.Goods.AddRange(
                G("Laptop Dell Latitude 5540","Laptop Dell Latitude 5540","Dell 노트북 5540",22000000),
                G("Màn hình LG 27 inch 4K","LG 27-inch 4K Monitor","LG 27인치 4K 모니터",8500000)
            );
            await context.SaveChangesAsync();
            Console.WriteLine("✅ Seeded 2 Goods");
        }

        // Warehouses
        if (!await context.Warehouses.AnyAsync())
        {
            context.Warehouses.AddRange(
                new Warehouse { Code="WH-001", Name="Kho Trung tâm TP.HCM", ManagerName="Phạm Thị Dung", CreatedDate=DateTime.Now, UserCreated=1 },
                new Warehouse { Code="WH-002", Name="Kho Hà Nội",            ManagerName="Vũ Văn Giang",  CreatedDate=DateTime.Now, UserCreated=1 }
            );
            await context.SaveChangesAsync();
            Console.WriteLine("✅ Seeded 2 Warehouses");
        }

        // Projects
        if (!await context.Projects.AnyAsync())
        {
            var adminId = (await context.Users.FirstOrDefaultAsync(u => u.Username == "admin"))?.Id ?? 1;
            context.Projects.AddRange(
                new Project { Code="PRJ-2025-001", Name="ACI Platform v2 - Nâng cấp toàn diện",
                    Description="Nâng cấp hạ tầng, UI/UX và tích hợp AI",
                    Status="active", StartDate=new DateTime(2025,1,1), EndDate=new DateTime(2025,12,31),
                    Budget=500000000, CompanyCode="ACI", CreatedAt=DateTime.Now, CreatedBy=adminId },
                new Project { Code="PRJ-2025-002", Name="Tích hợp AI & Tự động hoá quy trình",
                    Description="Nghiên cứu và tích hợp AI vào các nghiệp vụ kế toán, bán hàng",
                    Status="Planned", StartDate=new DateTime(2025,6,1), EndDate=new DateTime(2026,5,31),
                    Budget=300000000, CompanyCode="BHA", CreatedAt=DateTime.Now, CreatedBy=adminId }
            );
            await context.SaveChangesAsync();
            Console.WriteLine("✅ Seeded 2 Projects");
        }

        // ChartOfAccounts
        var year = DateTime.Now.Year;
        if (!await context.ChartOfAccounts.AnyAsync(c => c.Year == year))
            await ChartOfAccountSeeder.SeedAsync(context, year);

        Console.WriteLine("🎉 DatabaseSeeder completed!");
    }
}
