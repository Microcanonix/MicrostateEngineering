using MoleculeProcessDomain;
using ResearchDefinitionDomain.GamessCalculation;

namespace IMoleculeProcessFactory
{
    public interface IMoleculeWorkFlowFactory
    {
        List<MoleculeGmsWorkflow> BuildGmsWorkflow(MoleculesResearchDefinition researchDefinition);

    }
}
