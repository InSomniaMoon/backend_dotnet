using AutoMapper;
using GestionMateriel.Application;
using GestionMateriel.Application.Services;
using GestionMateriel.Infrastructure.Data;
using GestionMateriel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace GestionMateriel.Tests;

public static class TestHelper
{
    public static GestionMaterielDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<GestionMaterielDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var tenant = new Mock<ITenantProvider>();
        tenant.Setup(t => t.IsResolved).Returns(false);
        tenant.Setup(t => t.StructureMask).Returns((string?)null);
        tenant.Setup(t => t.StructureCode).Returns((string?)null);
        tenant.Setup(t => t.StructureId).Returns((int?)null);

        var jwtOpts = Options.Create(new JwtSettings
        {
            SecretKey = "test-secret-key-for-unit-tests",
            Issuer = "test",
            Audience = "test",
            ExpirationMinutes = 60,
            RefreshTokenExpirationDays = 7
        });

        return new NoFilterDbContext(options, tenant.Object, jwtOpts);
    }

    public static IMapper CreateMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly));
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

    // Skips tenant query filters — InMemory doesn't short-circuit || in expression trees
    // so navigation-property accesses inside filters fail when related entities are absent.
    private sealed class NoFilterDbContext(
        DbContextOptions<GestionMaterielDbContext> options,
        ITenantProvider tenantProvider,
        IOptions<JwtSettings> jwtSettings)
        : GestionMaterielDbContext(options, tenantProvider, jwtSettings)
    {
        protected override void ApplyTenantQueryFilters(ModelBuilder modelBuilder) { }
    }
}
