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
                services.AddTransient<IMoleculesFactory, MoleculesFactory>();
                services.AddTransient<IGmsCalcInputFactory, GmsCalcInputFactory>();
                services.AddTransient<IMoleculeReportFactory, MoleculeReportFactory>();
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IMoleculesFactory, MoleculesFactory>();
                services.AddScoped<IGmsCalcInputFactory, GmsCalcInputFactory>();
                services.AddScoped<IMoleculeReportFactory, MoleculeReportFactory>();

            }
            else if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IMoleculesFactory, MoleculesFactory>();
                services.AddSingleton<IGmsCalcInputFactory, GmsCalcInputFactory>();
                services.AddSingleton<IMoleculeReportFactory, MoleculeReportFactory>();
            }
            return services;
        }
    }
}
