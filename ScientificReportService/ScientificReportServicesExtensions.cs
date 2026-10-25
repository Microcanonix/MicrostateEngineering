using IScientificReportServices;
using Microsoft.Extensions.DependencyInjection;

namespace ScientificReportServices
{
    public static class ScientificReportServicesExtensions
    {
        public static IServiceCollection RegisterScientificReportService(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient<IScientificReportService, ScientificReportService>();
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped<IScientificReportService, ScientificReportService>();
            }
            else if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<IScientificReportService, ScientificReportService>();
            }
            return services;
        }
    }
}
