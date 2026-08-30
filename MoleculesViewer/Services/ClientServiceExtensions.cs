using Microsoft.Extensions.DependencyInjection;

namespace MoleculesViewer.Services
{
    public static class ClientServiceExtensions
    {
        public static IServiceCollection RegisterClientServices(this IServiceCollection services)
        {
            services.AddSingleton<DocumentService>();
            return services;
        }
    }
}
