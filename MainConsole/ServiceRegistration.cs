using Microsoft.Extensions.DependencyInjection;
using MoleculeFactory;
using MoleculeProcessService;
using MoleculeRepository;
using MoleculeServices;
using ResearchDefinitionRepository;
using ResearchDefinitionService;
using UtilityServices;
using MoleculeProcessFactory;

namespace MainConsole
{
    public static class ServiceRegistration
    {
        public static IServiceCollection Register(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            return services
                    .RegisterUtilities(serviceLifetime)
                        .RegisterResearchDefinitionRepo(serviceLifetime)
                        .RegisterResearchDefinitionSvc(serviceLifetime)
                        .RegisterMoleculeRepository(serviceLifetime)
                        .RegisterMoleculeFactory(serviceLifetime)
                        .RegisterMoleculeService(serviceLifetime)
                        .RegisterMoleculeProcessService(serviceLifetime)
                        .RegisterMoleculeProcessFactory(serviceLifetime);
                            
        }
    }
}
