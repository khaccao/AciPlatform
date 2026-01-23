using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Services;
using AciPlatform.Domain.Interfaces;
using AciPlatform.Infrastructure.Persistence;
using AciPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AciPlatform.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Add Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Add Services
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
