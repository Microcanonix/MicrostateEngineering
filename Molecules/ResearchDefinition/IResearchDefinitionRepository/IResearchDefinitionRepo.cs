using Microsoft.VisualBasic.FileIO;
using ResearchDefinitionDomain.GamessCalculation;

namespace IResearchDefinitionRepository
{
    public interface IResearchDefinitionRepo
    {
        List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions();

        MoleculesResearchDefinition? GetMoleculesResearchDefinition(string researchDefinitionName);

        void DeleteMoleculesResearchDefintion(string researchDefintionName);

        void SaveMoleculesResearchDefintion(MoleculesResearchDefinition researchDefintion);
    }
}
