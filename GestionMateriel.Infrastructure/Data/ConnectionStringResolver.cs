using Microsoft.Extensions.Configuration;

namespace GestionMateriel.Infrastructure.Data;

public static class ConnectionStringResolver
{
    public static string Resolve(IConfiguration configuration)
    {
        var explicitConnectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(explicitConnectionString))
            return explicitConnectionString;

        var host = configuration["DB_HOST"];
        var database = configuration["DB_DATABASE"];
        var username = configuration["DB_USERNAME"];
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(database) || string.IsNullOrWhiteSpace(username))
            throw new InvalidOperationException("DefaultConnection is missing and DB_HOST/DB_DATABASE/DB_USERNAME are not all set.");

        var port = configuration["DB_PORT"] ?? "5432";
        var password = configuration["DB_PASSWORD"] ?? string.Empty;

        return $"Host={host};Port={port};Database={database};Username={username};Password={password};";
    }
}
