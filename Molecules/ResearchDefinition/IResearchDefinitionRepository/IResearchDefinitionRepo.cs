using ResearchDefinitionDomain.GamessCalculation;

namespace IResearchDefinitionRepository
{
    public interface IResearchDefinitionRepo
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions();
    }
}
