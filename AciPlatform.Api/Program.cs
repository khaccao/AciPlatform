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
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Application.Services.FleetTransportation;
using AciPlatform.Application.Interfaces.MultiChannel;
using AciPlatform.Application.Services.MultiChannel;
using AciPlatform.Api.Filters;
using AciPlatform.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AciPlatform API", Version = "v1" });
    c.SwaggerDoc("customer", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Customer API", Version = "v1" });
    
    // Group controllers
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (docName == "customer")
        {
            var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];
            return controllerName == "Customers" || controllerName == "Auth";
        }
        return docName == "v1";
    });
});
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor(); // Required for ConnectionStringProvider

// Register Infrastructure Services
builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();

// Register Application Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IMenuRoleService, MenuRoleService>();
builder.Services.AddScoped<IUserMenuService, UserMenuService>();
builder.Services.AddScoped<IWebAuthService, WebAuthService>();
// builder.Services.AddScoped<IInvoiceAuthorize, InvoiceAuthorize>();
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
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarFleetService, CarFleetService>();
builder.Services.AddScoped<ICarFieldService, CarFieldService>();
builder.Services.AddScoped<ICarLocationService, CarLocationService>();
builder.Services.AddScoped<IDriverRouterService, DriverRouterService>();
builder.Services.AddScoped<IPetrolConsumptionService, PetrolConsumptionService>();
builder.Services.AddScoped<IPoliceCheckPointService, PoliceCheckPointService>();
builder.Services.AddScoped<IRoadRouteService, RoadRouteService>();
builder.Services.AddScoped<IFacebookService, FacebookService>();
builder.Services.AddScoped<IAIService, BasicAIService>();
builder.Services.AddScoped<IAutomationService, AutomationService>();
builder.Services.AddScoped<ITwoFactorService, TwoFactorService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<FleetExceptionFilter>();

// Configure DbContext with Dynamic Connection String
builder.Services.AddDbContext<AciPlatform.Infrastructure.Persistence.ApplicationDbContext>((sp, options) =>
{
    var connectionStringProvider = sp.GetRequiredService<IConnectionStringProvider>();
    var connectionString = connectionStringProvider.GetConnectionString();
    options.UseSqlServer(connectionString);
});

// Register IApplicationDbContext (Scoped)
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AciPlatform.Infrastructure.Persistence.ApplicationDbContext>());

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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/customer/swagger.json", "Customer API v1");
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Full API v1");
    c.RoutePrefix = "swagger";
});

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
        var context = services.GetRequiredService<AciPlatform.Infrastructure.Persistence.ApplicationDbContext>();
        context.Database.Migrate();
        Console.WriteLine("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred migrating the DB: {ex}");
    }
}

app.Run();




