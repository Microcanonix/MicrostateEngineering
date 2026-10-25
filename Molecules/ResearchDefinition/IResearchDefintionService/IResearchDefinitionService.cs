using ResearchDefinitionDomain.GamessCalculation;

namespace IResearchDefintionServices
{
    public interface IResearchDefinitionService
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions();

        MoleculesResearchDefinition? GetMoleculesResearchDefinition(string researchDefinitionName);

        void DeleteMoleculesResearchDefintion(string researchDefintionName);

        void SaveMoleculesResearchDefintion(MoleculesResearchDefinition researchDefintion);
    }
}
