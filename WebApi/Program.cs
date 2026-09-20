using ResearchDefinitionDomain.GamessCalculation;
using Serilog;

namespace WebApi
{
    public class Program
    {
        private const string DevelopmentCorsPolicy = "DevelopmentCors";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((hostContext, services, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(hostContext.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });

            builder.Services.Configure<ResearchDefinitionSettings>(
                builder.Configuration.GetSection(ResearchDefinitionSettings.Section));

            builder.Services.Register(ServiceLifetime.Singleton);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(DevelopmentCorsPolicy, policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    c.RoutePrefix = "swagger";
                });

                app.UseCors(DevelopmentCorsPolicy);
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
