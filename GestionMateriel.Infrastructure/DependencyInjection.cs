using System.Reflection;
using GestionMateriel.Application.Messaging;
using GestionMateriel.Application.Services;
using GestionMateriel.Infrastructure.Data;
using GestionMateriel.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionMateriel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = ConnectionStringResolver.Resolve(configuration);

        services.AddDbContext<GestionMaterielDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));
        services.Configure<EmailSettings>(configuration.GetSection("Email"));
        services.Configure<PasswordResetSettings>(configuration.GetSection("PasswordReset"));
        services.Configure<FrontendSettings>(configuration.GetSection("Frontend"));

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITenantProvider, TenantProvider>();
        services.AddScoped<IImageStorageService, LocalImageStorageService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();

        RegisterHandlers(services, typeof(DependencyInjection).Assembly);

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerInterface = typeof(IRequestHandler<,>);

        var handlers = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
                .Select(i => (ServiceType: i, ImplementationType: t)));

        foreach (var (serviceType, implementationType) in handlers)
        {
            services.AddScoped(serviceType, implementationType);
        }
    }
}
