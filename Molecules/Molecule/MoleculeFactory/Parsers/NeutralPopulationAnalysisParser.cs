using CoreDomain;
using MoleculeDomain;

namespace MoleculeFactory.Parsers
{
    internal class NeutralPopulationAnalysisParser : PopulationAnalysisParser
    {
        #region Tags

        private const string OptimizationResultTag = "     PROPERTY VALUES FOR THE RHF   SELF-CONSISTENT FIELD WAVEFUNCTION";

        private const string StartTag = "          MULLIKEN AND LOWDIN POPULATION ANALYSES";

        private const string StartTagAOPopulations = "               ----- POPULATIONS IN EACH AO -----";

        private const string StartTagOverlapPopulations = "          ----- MULLIKEN ATOMIC OVERLAP POPULATIONS -----";

        private const string StartTagPopulations = "          TOTAL MULLIKEN AND LOWDIN ATOMIC POPULATIONS";

        private const string StartTagBondOrder = "          BOND ORDER AND VALENCE ANALYSIS";

        #endregion


        #region abstract overrided

        protected override string GetGeometryResultTag()
        {
            return OptimizationResultTag;
        }

        protected override string GetStartTag()
        {
            return StartTag;
        }

        protected override string GetStartTagAOPopulations()
        {
            return StartTagAOPopulations;
        }

        protected override string GetStartTagBondOrder()
        {
            return StartTagBondOrder;
        }

        protected override string GetStartTagOverlapPopulations()
        {
            return StartTagOverlapPopulations;
        }

        protected override string GetStartTagPopulations()
        {
            return StartTagPopulations;
        }

        protected override PopulationAnalysisType GetPopulationStatus()
        {
            return PopulationAnalysisType.neutral;
        }

        #endregion



        internal static void GetPopulation(List<string> fileLines, Molecule molecule)
        {
            new NeutralPopulationAnalysisParser().Parse(fileLines, molecule);
        }
    }
}
