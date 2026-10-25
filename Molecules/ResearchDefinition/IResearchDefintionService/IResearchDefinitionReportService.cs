using ResearchDefinitionDomain.GamessCalculation.Report;

namespace IResearchDefintionServices
{
    public interface IResearchDefinitionReportService
    {
        MoleculeResearchDefinitionReport? Read(string name);

        void Save(MoleculeResearchDefinitionReport report);
    }
}
