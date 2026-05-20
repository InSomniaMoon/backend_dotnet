using System.Reflection;
using MediatR;

namespace MediatR
{
    public sealed class MediatorServiceConfiguration
    {
        private readonly HashSet<Assembly> _assemblies = [];

        internal IReadOnlyCollection<Assembly> Assemblies => _assemblies;

        public void RegisterServicesFromAssembly(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }
    }

    internal sealed class SimpleMediator(IServiceProvider serviceProvider) : IMediator
    {
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var handler = serviceProvider.GetService(handlerType);

            if (handler is null)
            {
                throw new InvalidOperationException(
                    $"No handler registered for request type '{requestType.FullName}'.");
            }

            var handleMethod = handlerType.GetMethod("Handle");

            if (handleMethod is null)
            {
                throw new InvalidOperationException(
                    $"The handler for '{requestType.FullName}' does not expose a compatible Handle method.");
            }

            var responseTask = handleMethod.Invoke(handler, [request, cancellationToken]) as Task<TResponse>;

            if (responseTask is null)
            {
                throw new InvalidOperationException(
                    $"The handler for '{requestType.FullName}' returned an invalid response task.");
            }

            return await responseTask.ConfigureAwait(false);
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MediatorServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatR(
            this IServiceCollection services,
            Action<MediatorServiceConfiguration>? configure = null)
        {
            var configuration = new MediatorServiceConfiguration();
            configure?.Invoke(configuration);

            foreach (var assembly in configuration.Assemblies)
            {
                RegisterHandlersFromAssembly(services, assembly);
            }

            services.AddTransient<IMediator, SimpleMediator>();
            services.AddTransient<ISender>(sp => sp.GetRequiredService<IMediator>());

            return services;
        }

        private static void RegisterHandlersFromAssembly(IServiceCollection services, Assembly assembly)
        {
            var handlerInterfaceType = typeof(IRequestHandler<,>);

            var handlerTypes = assembly
                .GetTypes()
                .Where(type => type is { IsClass: true, IsAbstract: false });

            foreach (var handlerType in handlerTypes)
            {
                var serviceTypes = handlerType
                    .GetInterfaces()
                    .Where(@interface =>
                        @interface.IsGenericType &&
                        @interface.GetGenericTypeDefinition() == handlerInterfaceType);

                foreach (var serviceType in serviceTypes)
                {
                    services.AddTransient(serviceType, handlerType);
                }
            }
        }
    }
}
