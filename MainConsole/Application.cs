using Microsoft.Extensions.Hosting;

namespace MainConsole
{
    public sealed class Application : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;

        }
    }
}
