
using ResearchDefinitionDomain.GamessCalculation.Report;

namespace IMoleculeProcessServices
{
    public interface IMoleculeWorkflowService
    {
        Task RunAsync();
        Task<MoleculeResearchDefinitionReport?> RunAsync(string researchDefintionName);
    }

}
