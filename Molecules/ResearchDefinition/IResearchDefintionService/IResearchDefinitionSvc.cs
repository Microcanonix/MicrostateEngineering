using ResearchDefinitionDomain.Molecule;

namespace IResearchDefintionService
{
    public interface IResearchDefinitionSvc
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions();
    }
}
