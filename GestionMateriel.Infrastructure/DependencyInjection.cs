using GestionMateriel.Application.Services;
using GestionMateriel.Domain.Interfaces;
using GestionMateriel.Infrastructure.Data;
using GestionMateriel.Infrastructure.Data.Repositories;
using GestionMateriel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionMateriel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is missing.");

        services.AddDbContext<GestionMaterielDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
        services.AddScoped<IItemIssueRepository, ItemIssueRepository>();
        services.AddScoped<IItemIssueCommentRepository, ItemIssueCommentRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventSubscriptionRepository, EventSubscriptionRepository>();
        services.AddScoped<IStructureRepository, StructureRepository>();
        services.AddScoped<IUserStructureRepository, UserStructureRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITenantProvider, TenantProvider>();

        return services;
    }
}
