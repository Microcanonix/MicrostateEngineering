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
                    services.AddScoped<IResearchDefinitionReportService, ResearchDefinitionReportService>();
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient<IResearchDefinitionService, ResearchDefinitionService>();
                    services.AddTransient<IResearchDefinitionReportService, ResearchDefinitionReportService>();
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton<IResearchDefinitionService, ResearchDefinitionService>();
                    services.AddSingleton<IResearchDefinitionReportService, ResearchDefinitionReportService>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, "Unsupported lifetime");
            }

            return services;

        }
    }
}
