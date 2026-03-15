using IMoleculeProcessServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MainConsole
{
    public sealed class Application : BackgroundService
    {
        private readonly ILogger<Application> _logger;

        private readonly IEnumerable<IMoleculeWorkflowService> _workflowRunners;

        public Application(ILogger<Application> logger, 
                        IEnumerable<IMoleculeWorkflowService> workFlowRunners)
        {
            _logger = logger;
            _workflowRunners = workFlowRunners;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Application started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Application running at: {time}", DateTimeOffset.Now);
                foreach (var workflow in _workflowRunners)
                {
                    await workflow.RunAsync();
                }
                await Task.Delay(60000, stoppingToken);
            }
            _logger.LogInformation("Application stopping");

        }
    }
}
