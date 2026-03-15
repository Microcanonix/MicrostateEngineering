using IMoleculeProcessFactory;
using Microsoft.Extensions.DependencyInjection;

namespace MoleculeProcessFactory
{
    public static class MoleculeProcessFactoryExtensions
    {
        public static IServiceCollection RegisterMoleculeProcessFactory(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient<IMoleculeWorkFlowFactory, MoleculeWorkFlowFactory>();
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IMoleculeWorkFlowFactory, MoleculeWorkFlowFactory>();
            }
            else if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IMoleculeWorkFlowFactory, MoleculeWorkFlowFactory>();
            }
            return services;
        }
    }
}
