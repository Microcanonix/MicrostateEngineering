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
                services.AddTransient<IGmsInputService, GmsInputService>();
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IMoleculeService, MoleculeService>();
                services.AddScoped<IGmsInputService, GmsInputService>();
            }
            else if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IMoleculeService, MoleculeService>();
                services.AddSingleton<IGmsInputService, GmsInputService>();
            }
            return services;
        }
    }
}
