using MoleculeDomain;
using MoleculeFactory.Conversion;

namespace MoleculeFactory.Parsers
{
    public static class GeoOptDftEnergyParser
    {
        private const string OptimizationResultTag = "***** EQUILIBRIUM GEOMETRY LOCATED *****";
        private const string EnergyStartTag = "          ENERGY COMPONENTS";
        private const string EnergyTag = "                       TOTAL ENERGY";

        public static void Parse(List<string> fileLines, Molecule molecule)
        {
            bool start = false;
            bool overallstart = false;
            for (int c = 0; c < fileLines.Count; ++c)
            {
                string line = fileLines[c];
                if (line.Contains(OptimizationResultTag))
                {
                    overallstart = true;
                }

                if (overallstart && line.Contains(EnergyStartTag))
                {
                    start = true;
                }

                if (start && line.Contains(EnergyTag))
                {
                    var data = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (data.Length > 1)
                    {
                        molecule.DftEnergy = StringConversion.ToDouble(data[1].Trim());

                        break;
                    }
                }
            }
        }

    }
}
