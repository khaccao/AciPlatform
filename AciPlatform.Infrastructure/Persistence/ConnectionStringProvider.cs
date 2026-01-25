using AciPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AciPlatform.Infrastructure.Persistence;

public class ConnectionStringProvider : IConnectionStringProvider
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ConnectionStringProvider(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetConnectionString(string? databaseName = null)
    {
        var connectionStringPlaceHolder = _configuration.GetConnectionString("ConnStr");
        if (string.IsNullOrEmpty(connectionStringPlaceHolder))
        {
            return _configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        var dbName = string.IsNullOrEmpty(databaseName) 
            ? GetDbName()
            : databaseName;

        var finalConnStr = connectionStringPlaceHolder.Replace("{dbName}", dbName);
        Console.WriteLine($"DEBUG: Connection String: {finalConnStr}");
        return finalConnStr;
    }

    public string GetDbName()
    {
        var dbNameFromConfiguration = _configuration.GetConnectionString("DbName");
        if (_configuration.GetConnectionString("isMultiDb") != "1")
        {
            return dbNameFromConfiguration ?? "";
        }

        try
        {
            var dbNameFromRequest = _httpContextAccessor.HttpContext?.Request?.Headers["dbName"].FirstOrDefault();

            return string.IsNullOrEmpty(dbNameFromRequest) 
                    ? dbNameFromConfiguration ?? ""
                    : dbNameFromRequest;
        }
        catch
        {
            return dbNameFromConfiguration ?? "";
        }
    }
}
