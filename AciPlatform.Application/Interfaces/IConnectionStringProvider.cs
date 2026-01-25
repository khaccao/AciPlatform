namespace AciPlatform.Application.Interfaces;

public interface IConnectionStringProvider
{
    string GetConnectionString(string? databaseName = null);
    string GetDbName();
}
