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
                    services.AddScoped<IResearchDefinitionSvc, ResearchDefinitionSvc>();
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient<IResearchDefinitionSvc, ResearchDefinitionSvc>();
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton<IResearchDefinitionSvc, ResearchDefinitionSvc>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, "Unsupported lifetime");
            }

            return services;

        }
    }
}
