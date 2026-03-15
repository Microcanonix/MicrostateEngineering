using MoleculeDomain;

namespace MoleculeFactory.Parsers
{
    public class GmsCalcValidityParser
    {
        private const string GamessCalcNormalExecution = "EXECUTION OF GAMESS TERMINATED NORMALLY";

        private const string SCFNoConverge = "SCF IS UNCONVERGED";

        public static bool TryParse(List<string> fileLines, Molecule molecule)
        {
            if (!fileLines.Exists(i => i.Contains(GamessCalcNormalExecution)))
            {
                return false;
            }

            if (fileLines.Exists(fileLines => fileLines.Contains(SCFNoConverge)))
            {
                return false;
            }

            return true;
        }


    }
}
