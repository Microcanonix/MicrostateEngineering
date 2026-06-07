using ResearchDefinitionDomain.GamessCalculation;

namespace IResearchDefintionService
{
    public interface IResearchDefinitionService
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions(string sourcePath);
    }
}
