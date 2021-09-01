using Microsoft.Extensions.DependencyInjection;

namespace ExtensionPrototype
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationInsightsExtension(this IServiceCollection services)
        {
            _ = new AzureFunctionsDiagnosticsEventHandler();

            return services;
        }
    }
}
