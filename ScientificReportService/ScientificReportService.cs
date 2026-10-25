using IResearchDefintionServices;
using IScientificReportServices;
using ScientificReportDomain;

namespace ScientificReportServices
{
    public sealed class ScientificReportService(IResearchDefinitionService researchDefintionService) : IScientificReportService
    {
        public ReseachDefinitionScientificReport? GetResearchDefinitionReport(string researchDefinitionName)
        {
            throw new NotImplementedException();
        }

        public List<ReseachDefinitionScientificReport> GetResearchDefinitionReports()
        {
            throw new NotImplementedException();
        }
    }
}
