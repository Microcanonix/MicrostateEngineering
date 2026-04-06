using ResearchDefinitionDomain.Report;

namespace IResearchDefintionService
{
    public interface IResearchDefinitionReportService
    {
        MoleculeResearchDefinitionReport? Read(string name);

        void Save(MoleculeResearchDefinitionReport report);
    }
}
