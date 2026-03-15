using IMoleculeServices;
using Microsoft.Extensions.DependencyInjection;

namespace MoleculeServices
{
    public static class MoleculeServiceExtensions
    {
        public static IServiceCollection RegisterMoleculeService(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient<IMoleculeService, MoleculeService>();
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IMoleculeService, MoleculeService>();
            }
            else if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IMoleculeService, MoleculeService>();
            }
            return services;
        }
    }
}
