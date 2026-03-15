using IMoleculeProcessServices;
using Microsoft.Extensions.DependencyInjection;

namespace MoleculeProcessService
{
    public static class MoleculeProcessServiceExtensions
    {
        public static IServiceCollection RegisterMoleculeProcessService(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient<IMoleculeProcessService, MoleculeProcessService>();
                services.AddTransient<IMoleculeWorkflowService, MoleculeGmsWorkflowService>();

            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IMoleculeProcessService, MoleculeProcessService>();
                services.AddScoped<IMoleculeWorkflowService, MoleculeGmsWorkflowService>();
            }
            else if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IMoleculeProcessService, MoleculeProcessService>();
                services.AddScoped<IMoleculeWorkflowService, MoleculeGmsWorkflowService>();
            }
            return services;
        }
    }
}
