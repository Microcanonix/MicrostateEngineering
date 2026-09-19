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



        public ResearchDefinitionService(IResearchDefinitionRepo repository 
                                , ILogger<ResearchDefinitionService> logger )
        {
            _logger = logger;
            _repository = repository;
        }

        public List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions()
        {
            try
            {
                return _repository.GetMoleculesResearchDefinitions();
            }
            catch(Exception e)
            {
                _logger.LogCritical(e, "Error while retrieving research definition");
                throw;
            }
        }

        public void DeleteMoleculesResearchDefintion(string researchDefintionName)
        {
            try
            {
                _repository.DeleteMoleculesResearchDefintion(researchDefintionName);
            }
            catch(Exception e)
            {
                _logger.LogCritical(e, "Error while deleting research definition");
                throw;
            }
        }

        public void SaveMoleculesResearchDefintion(MoleculesResearchDefinition researchDefintion)
        {
            try
            {
                _repository.SaveMoleculesResearchDefintion(researchDefintion);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error while deleting research definition");
                throw;
            }
        }
    }
}
