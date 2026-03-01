using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResearchDefinitionDomain.Settings;

namespace MainConsole
{
    public class Program
    {
        static void Main(string[] args)
        {

            var builder = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                services.Configure<ResearchDefinitionSettings>(hostContext.Configuration.GetSection(ResearchDefinitionSettings.Section));
                services.Register(ServiceLifetime.Singleton);
                services.AddHostedService<Application>();
            });

            builder.ConfigureAppConfiguration((hostContext, options) =>
               {
                   options.AddEnvironmentVariables();
                   options.AddCommandLine(args);
                   options.AddJsonFile("./appsettings.json", optional: false);
               });

            var host = builder.Build();
            host.Run();
        }
    }
}
