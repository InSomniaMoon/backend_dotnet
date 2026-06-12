using Microsoft.Extensions.DependencyInjection;

namespace GestionMateriel.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly));
        return services;
    }
}
