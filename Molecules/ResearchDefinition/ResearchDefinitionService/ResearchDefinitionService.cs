using IResearchDefinitionRepository;
using IResearchDefintionService;
using Microsoft.Extensions.Logging;
using ResearchDefinitionDomain.GamessCalculation;

namespace ResearchDefinitionService
{
    public sealed class ResearchDefinitionService : IResearchDefinitionService
    {
        private readonly ILogger<ResearchDefinitionService> _logger;

        private readonly IResearchDefinitionRepo        _repository;

        public ResearchDefinitionService(
                            IResearchDefinitionRepo repository,
                            ILogger<ResearchDefinitionService> logger
                                )
        {
            _logger = logger;
            _repository = repository;
        }

        public List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions()
        {
            _logger.LogInformation($"{nameof(GetMoleculesResearchDefinitions)}");
            return _repository.GetMoleculesResearchDefinitions();
        }
    }
}
