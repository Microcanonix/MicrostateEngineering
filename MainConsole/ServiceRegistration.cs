using Microsoft.Extensions.DependencyInjection;
using ResearchDefinitionRepository;
using ResearchDefinitionService;
using UtilityServices;

namespace MainConsole
{
    public static class ServiceRegistration
    {
        public static IServiceCollection Register(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            return services
                    .RegisterUtilities(serviceLifetime)
                        .RegisterResearchDefinitionRepo(serviceLifetime)
                        .RegisterResearchDefinitionSvc(serviceLifetime);
                            
        }
    }
}
