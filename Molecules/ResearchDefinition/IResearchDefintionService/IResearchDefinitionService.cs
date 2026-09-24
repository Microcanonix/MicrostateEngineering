using ResearchDefinitionDomain.GamessCalculation;

namespace IResearchDefintionService
{
    public interface IResearchDefinitionService
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions();

        MoleculesResearchDefinition? GetMoleculesResearchDefinition(string researchDefinitionName);

        void DeleteMoleculesResearchDefintion(string researchDefintionName);

        void SaveMoleculesResearchDefintion(MoleculesResearchDefinition researchDefintion);
    }
}
