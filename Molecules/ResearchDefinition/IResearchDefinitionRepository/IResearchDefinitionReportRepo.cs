using ResearchDefinitionDomain.Report;

namespace IResearchDefinitionRepository
{
    public interface IResearchDefinitionReportRepo
    {
        void Save(MoleculeResearchDefinitionReport report);

        MoleculeResearchDefinitionReport? Read(string name);

    }
}
