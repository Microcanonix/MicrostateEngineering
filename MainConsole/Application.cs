using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MainConsole
{
    public sealed class Application : BackgroundService
    {
        private readonly ILogger<Application> _logger;

        public Application(ILogger<Application> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Application started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Application running at: {time}", DateTimeOffset.Now);
                await Task.Delay(2000, stoppingToken);
            }

            _logger.LogInformation("Application stopping");

        }
    }
}
