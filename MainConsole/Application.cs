using IMoleculeProcessServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ResearchDefinitionDomain.GamessCalculation;

namespace MainConsole
{
    public sealed class Application : BackgroundService
    {
        private readonly ILogger<Application> _logger;

        private readonly IEnumerable<IMoleculeWorkflowService> _workflowRunners;

        private readonly ResearchDefinitionSettings _settings;

        public Application(ILogger<Application> logger
                        , IEnumerable<IMoleculeWorkflowService> workFlowRunners
                        , IOptions<ResearchDefinitionSettings> settings)
        {
            _logger = logger;
            _workflowRunners = workFlowRunners;
            _settings = settings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Application started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Application running at: {time}", DateTimeOffset.Now);
                foreach (var workflow in _workflowRunners)
                {
                    await workflow.RunAsync(_settings.MoleculesLocation);
                }
                await Task.Delay(3600000, stoppingToken);
            }
            _logger.LogInformation("Application stopping");

        }
    }
}
