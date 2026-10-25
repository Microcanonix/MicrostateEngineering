using MoleculeFactory;
using MoleculeProcessFactory;
using MoleculeProcessService;
using MoleculeRepository;
using MoleculeServices;
using ResearchDefinitionRepository;
using ResearchDefinitionServices;
using UtilityServices;

namespace WebApi
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
