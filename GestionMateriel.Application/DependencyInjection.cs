using System.Reflection;
using GestionMateriel.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace GestionMateriel.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly));
        RegisterHandlers(services, typeof(ApplicationAssemblyMarker).Assembly);
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
            services.AddTransient(serviceType, implementationType);
        }
    }
}
