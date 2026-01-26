using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Services;
using AciPlatform.Application.Services.HoSoNhanSu;
using AciPlatform.Application.Services.LuongPhucLoi;
using AciPlatform.Application.Services.HopDong;
using AciPlatform.Application.Services.ChamCong;
using AciPlatform.Application.Interfaces.LuongPhucLoi;
using AciPlatform.Application.Interfaces.HopDong;
using AciPlatform.Application.Interfaces.ChamCong;
using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor(); // Required for ConnectionStringProvider

// Register Infrastructure Services
builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();

// Register Application Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IWebAuthService, WebAuthService>();
builder.Services.AddScoped<IInvoiceAuthorize, InvoiceAuthorize>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionDetailService, PositionDetailService>();
builder.Services.AddScoped<IDegreeService, DegreeService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IMajorService, MajorService>();
builder.Services.AddScoped<IRelativeService, RelativeService>();
builder.Services.AddScoped<IHistoryAchievementService, HistoryAchievementService>();
builder.Services.AddScoped<IDecisionTypeService, DecisionTypeService>();
builder.Services.AddScoped<IDecideService, DecideService>();
builder.Services.AddScoped<IAllowanceService, AllowanceService>();
builder.Services.AddScoped<IAllowanceUserService, AllowanceUserService>();
builder.Services.AddScoped<ISalaryTypeService, SalaryTypeService>();
builder.Services.AddScoped<IContractTypeService, ContractTypeService>();
builder.Services.AddScoped<IContractFileService, ContractFileService>();
builder.Services.AddScoped<IUserContractHistoryService, UserContractHistoryService>();
builder.Services.AddScoped<ITimeKeepingService, TimeKeepingService>();
builder.Services.AddScoped<IUserCompanyService, UserCompanyService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// Configure DbContext with Dynamic Connection String
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var connectionStringProvider = sp.GetRequiredService<IConnectionStringProvider>();
    var connectionString = connectionStringProvider.GetConnectionString();
    options.UseSqlServer(connectionString);
});

// Register IApplicationDbContext (Scoped)
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "SuperSecretKeyDefault123!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // Validate in production
        ValidateAudience = false, // Validate in production
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure Authentication middleware is added
app.UseAuthorization();

app.MapControllers();

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        Console.WriteLine("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred migrating the DB: {ex.Message}");
    }
}

app.Run();
