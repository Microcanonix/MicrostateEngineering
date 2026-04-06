using IResearchDefinitionRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ResearchDefinitionRepository
{
    public static class ResearchDefinitionExtensions
    {
        public static IServiceCollection RegisterResearchDefinitionRepo(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped<IResearchDefinitionRepo, ResearchDefinitionRepo>();
                    services.AddScoped<IResearchDefinitionReportRepo, ResearchDefinitionReportRepo>();
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient<IResearchDefinitionRepo, ResearchDefinitionRepo>();
                    services.AddTransient<IResearchDefinitionReportRepo, ResearchDefinitionReportRepo>();
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton<IResearchDefinitionRepo, ResearchDefinitionRepo>();
                    services.AddSingleton<IResearchDefinitionReportRepo, ResearchDefinitionReportRepo>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, "Unsupported lifetime");
            }

            return services;

        }
    }
}
