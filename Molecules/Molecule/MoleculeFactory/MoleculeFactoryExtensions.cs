using IMoleculeFactory;
using Microsoft.Extensions.DependencyInjection;

namespace MoleculeFactory
{
    public static class MoleculeFactoryExtensions
    {
        public static IServiceCollection RegisterMoleculeFactory(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient<IBuildMoleculeFactory, BuildMoleculeFactory>();
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IBuildMoleculeFactory, BuildMoleculeFactory>();

            }
            else if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IBuildMoleculeFactory, BuildMoleculeFactory>();
            }
            return services;
        }
    }
}
