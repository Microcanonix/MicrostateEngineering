using IUtilitiesServices;
using Microsoft.Extensions.DependencyInjection;

namespace UtilityServices
{
    public static class UtilitiesExtensions
    {
        public static IServiceCollection RegisterUtilities(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped<IDirectoryServices, DirectoryServices>();
                    services.AddScoped<IFileServices, FileServices>();
                    services.AddScoped(typeof(IJsonParser<>), typeof(JsonParser<>));
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient<IDirectoryServices, DirectoryServices>();
                    services.AddTransient<IFileServices, FileServices>();
                    services.AddTransient(typeof(IJsonParser<>), typeof(JsonParser<>));
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton<IDirectoryServices, DirectoryServices>();
                    services.AddSingleton<IFileServices, FileServices>();
                    services.AddSingleton(typeof(IJsonParser<>), typeof(JsonParser<>));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, "Unsupported lifetime");
            }

            return services;
        }
    }
}
