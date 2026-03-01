using ResearchDefinitionDomain.Molecule;

namespace IResearchDefinitionRepository
{
    public interface IResearchDefinitionRepo
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions();
    }
}
