using ResearchDefinitionDomain.GamessCalculation;

namespace IResearchDefintionService
{
    public interface IResearchDefinitionService
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions();

        void DeleteMoleculesResearchDefintion(string researchDefintionName);

        void SaveMoleculesResearchDefintion(MoleculesResearchDefinition researchDefintion);
    }
}
