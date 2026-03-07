using IMoleculeRepository;
using Microsoft.Extensions.DependencyInjection;

namespace MoleculeRepository
{
    public static class MoleculeRepositoryExtensions
    {

        private static IServiceCollection RegisterCoreRepository(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient<IMoleculeGmsOutputRepository, MoleculeGmsOutputRepository>();
                services.AddTransient<IMoleculeGmsInputRepository, MoleculeGmsInputRepository>();
                services.AddTransient<IMoleculeDataRepository, MoleculeDataRepository>();
                services.AddTransient<IMoleculeXyzRepository, MoleculeXyzRepository>();
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IMoleculeGmsOutputRepository, MoleculeGmsOutputRepository>();
                services.AddScoped<IMoleculeGmsInputRepository, MoleculeGmsInputRepository>();
                services.AddScoped<IMoleculeDataRepository, MoleculeDataRepository>();
                services.AddScoped<IMoleculeXyzRepository, MoleculeXyzRepository>();
            }
            else if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IMoleculeGmsOutputRepository, MoleculeGmsOutputRepository>();
                services.AddSingleton<IMoleculeGmsInputRepository, MoleculeGmsInputRepository>();
                services.AddSingleton<IMoleculeDataRepository, MoleculeDataRepository>();
                services.AddSingleton<IMoleculeXyzRepository, MoleculeXyzRepository>();
            }
            return services;
        }
    }
}
