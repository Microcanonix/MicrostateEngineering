namespace MoleculeFactory.Parsers
{
    internal class FukuiEnergyNeutralParser : FukuiEnergyParser
    {
        #region tags


        private const string StartTag = "     PROPERTY VALUES FOR THE RHF   SELF-CONSISTENT FIELD WAVEFUNCTION";


        private const string EnergyTag = "TOTAL ENERGY";

        #endregion


        protected override string GetEnergyTag()
        {
            return StartTag;
        }

        protected override string GetStartTag()
        {
            return EnergyTag;
        }


        internal static double GetEnergy(List<string> fileLines)
        {
            FukuiEnergyNeutralParser parser = new();
            return parser.Parse(fileLines);
        }
    }
}
