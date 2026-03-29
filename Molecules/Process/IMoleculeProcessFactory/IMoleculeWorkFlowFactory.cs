using MoleculeProcessDomain;
using ResearchDefinitionDomain;

namespace IMoleculeProcessFactory
{
    public interface IMoleculeWorkFlowFactory
    {
        List<MoleculeGmsWorkflow> BuildGmsWorkflow(MoleculesResearchDefinition researchDefinition);

    }
}
