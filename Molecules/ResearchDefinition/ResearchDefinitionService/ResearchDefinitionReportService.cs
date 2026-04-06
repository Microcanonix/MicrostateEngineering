using IResearchDefinitionRepository;
using IResearchDefintionService;
using Microsoft.Extensions.Logging;
using ResearchDefinitionDomain.Report;

namespace ResearchDefinitionService
{
    public sealed class ResearchDefinitionReportService : IResearchDefinitionReportService
    {
        private readonly IResearchDefinitionReportRepo _researchDefinitionReportRepo;

        private readonly ILogger<ResearchDefinitionReportService> _logger;

        public ResearchDefinitionReportService(IResearchDefinitionReportRepo researchDefinitionReportRepo,
                                                ILogger<ResearchDefinitionReportService> logger)
        {
            _researchDefinitionReportRepo = researchDefinitionReportRepo;
            _logger = logger;
        }

        public MoleculeResearchDefinitionReport? Read(string name)
        {
            try
            {
               return _researchDefinitionReportRepo.Read(name);
            }
            catch(Exception e)
            {
                _logger.LogCritical(e, $"Error while reading {nameof(MoleculeResearchDefinitionReport)} with name {name}");
                throw;
            }
        }

        public void Save(MoleculeResearchDefinitionReport report)
        {
            try
            {
                _researchDefinitionReportRepo.Save(report);
            }
            catch(Exception e)
            {
                _logger.LogCritical(e, $"Error while saving {nameof(MoleculeResearchDefinitionReport)} with name {report.Name}");
                throw;
            }
        }
    }
}
