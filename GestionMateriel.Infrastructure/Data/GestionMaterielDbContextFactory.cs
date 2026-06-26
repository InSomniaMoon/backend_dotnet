using System.Text.Json;
using GestionMateriel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        return ConnectionStringResolver.Resolve(configuration);
    }
}