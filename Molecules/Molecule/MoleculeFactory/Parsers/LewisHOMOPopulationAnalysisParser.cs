using CoreFactories.Parsers;
using MoleculeDomain;

namespace MoleculeFactory.Parsers
{
    internal class LewisHOMOPopulationAnalysisParser : UHFPopulationAnalysisParser
    {
        protected override PopulationAnalysisType GetPopulationStatus()
        {
            return PopulationAnalysisType.lewisHOMO;
        }

        internal static void GetPopulation(List<string> fileLines, Molecule molecule)
        {
            LewisHOMOPopulationAnalysisParser parser = new();
            parser.Parse(fileLines, molecule);
        }
    }
}
