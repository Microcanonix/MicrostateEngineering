using ResearchDefinitionDomain;

namespace IResearchDefinitionRepository
{
    public interface IResearchDefinitionRepo
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions();
    }
}
