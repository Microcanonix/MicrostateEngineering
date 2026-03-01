using IResearchDefinitionRepository;
using IResearchDefintionService;
using Microsoft.Extensions.Logging;
using ResearchDefinitionDomain.Molecule;

namespace ResearchDefinitionService
{
    public sealed class ResearchDefinitionSvc : IResearchDefinitionSvc
    {
        private readonly ILogger<ResearchDefinitionSvc> _logger;

        private readonly IResearchDefinitionRepo        _repository;

        public ResearchDefinitionSvc(
                            IResearchDefinitionRepo repository,
                            ILogger<ResearchDefinitionSvc> logger
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
