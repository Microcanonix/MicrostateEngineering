using IResearchDefintionService;
using Microsoft.Extensions.DependencyInjection;

namespace ResearchDefinitionService
{
    public static class ResearchDefinitionExtensions
    {
        public static IServiceCollection RegisterResearchDefinitionSvc(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {

            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped<IResearchDefinitionService, ResearchDefinitionService>();
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient<IResearchDefinitionService, ResearchDefinitionService>();
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton<IResearchDefinitionService, ResearchDefinitionService>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, "Unsupported lifetime");
            }

            return services;

        }
    }
}
