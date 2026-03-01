using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResearchDefinitionDomain.Settings;
using Serilog;

namespace MainConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            builder.UseSerilog((hostContext, services, loggerConfig) =>
             {
                 loggerConfig
                     .ReadFrom.Configuration(hostContext.Configuration)
                     .ReadFrom.Services(services)
                     .Enrich.FromLogContext()
                     .WriteTo.Console();
             });

            builder.ConfigureServices((hostContext, services) =>
            {
                services.Configure<ResearchDefinitionSettings>(hostContext
                    .Configuration
                    .GetSection(ResearchDefinitionSettings.Section));
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
