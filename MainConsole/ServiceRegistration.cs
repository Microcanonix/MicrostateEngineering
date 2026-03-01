using Microsoft.Extensions.DependencyInjection;
using UtilityServices;

namespace MainConsole
{
    public static class ServiceRegistration
    {
        public static IServiceCollection Register(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            return services.RegisterUtilities(serviceLifetime);
                            
        }
    }
}
