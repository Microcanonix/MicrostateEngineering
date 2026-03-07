using MoleculeFactory.Parsers;

namespace CoreFactories.Parsers
{
    internal abstract class UHFPopulationAnalysisParser : PopulationAnalysisParser
    {
        #region Tags

        private const string OptimizationResultTag = "          ********* ALL ELECTRONS ********";

        private const string StartTag = OptimizationResultTag;

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


        #endregion
    }
}
