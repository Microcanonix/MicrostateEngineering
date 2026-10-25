using ScientificReportDomain;

namespace IScientificReportServices
{
    public interface IScientificReportService
    {
        List<ReseachDefinitionScientificReport> GetResearchDefinitionReports();


        ReseachDefinitionScientificReport? GetResearchDefinitionReport(string researchDefinitionName);
    }
}
