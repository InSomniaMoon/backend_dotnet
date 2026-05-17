using System.Text.Json;
using GestionMateriel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace GestionMateriel.Infrastructure.Data;

public class GestionMaterielDbContextFactory : IDesignTimeDbContextFactory<GestionMaterielDbContext>
{
    public GestionMaterielDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<GestionMaterielDbContext>();
        var connectionString = ResolveConnectionString();

        optionsBuilder.UseNpgsql(connectionString);

        var tenantProvider = new TenantProvider();
        var jwtSettings = Options.Create(new JwtSettings
        {
            Structure = new StructureSettings
            {
                Role = string.Empty
            }
        });

        return new GestionMaterielDbContext(optionsBuilder.Options, tenantProvider, jwtSettings);
    }

    private static string ResolveConnectionString()
    {
        var fromEnv = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (!string.IsNullOrWhiteSpace(fromEnv))
        {
            return fromEnv;
        }

        var appSettingsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../GestionMateriel.Presentation/appsettings.json"));
        if (!File.Exists(appSettingsPath))
        {
            throw new InvalidOperationException(
                "Unable to resolve connection string for EF Core design-time. Set ConnectionStrings__DefaultConnection environment variable.");
        }

        using var stream = File.OpenRead(appSettingsPath);
        using var document = JsonDocument.Parse(stream);

        if (!document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings)
            || !connectionStrings.TryGetProperty("DefaultConnection", out var defaultConnection)
            || string.IsNullOrWhiteSpace(defaultConnection.GetString()))
        {
            throw new InvalidOperationException(
                "ConnectionStrings:DefaultConnection is missing in GestionMateriel.Presentation/appsettings.json.");
        }

        return defaultConnection.GetString()!;
    }
}