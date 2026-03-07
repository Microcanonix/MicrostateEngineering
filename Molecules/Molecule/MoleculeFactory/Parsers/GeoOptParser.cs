using CoreDomain;
using MoleculeDomain;
using MoleculeDomain.Utilities;
using MoleculeFactory.Conversion;

namespace CoreFactories.Parsers
{
    public class GeoOptParser
    {
        private const string OptimizationResultTag = "***** EQUILIBRIUM GEOMETRY LOCATED *****";

        public static void Parse(List<string> fileLines, Molecule molecule)
        {
            bool start = false;
            int position = 1;

            if (!IsValid(fileLines))
            {
                return;
            }

            for (int c = 0; c < fileLines.Count; ++c)
            {
                var line = fileLines[c];
                if (line.Contains(OptimizationResultTag))
                {
                    start = true;
                    c += 3;
                    continue;
                }

                if (start)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }

                    var newatom = ParseOptAtomPosition(line);
                    newatom.PositionInMolecule = position;
                    var existingAtomIndex = molecule.Atoms.FindIndex((i) => i.Atom.Symbol == newatom.Atom.Symbol
                                                                            && i.PositionInMolecule == position);
                    if (existingAtomIndex != -1)
                    {
                        molecule.Atoms[existingAtomIndex] = newatom;
                    }
                    else
                    {
                        molecule.Atoms.Add(newatom);
                    }
                    ++position;
                }
            }
        }



        private static bool IsValid(List<string> fileLines)
        {
            return fileLines.Exists((i) => i.Contains(OptimizationResultTag));
        }


        private static MoleculeAtom ParseOptAtomPosition(string line)
        {
            var current = FindNthSegment(line, 1);
            string atomsymbol = line[current.Item1..current.Item2];
            current = FindNthSegment(line, 2);
            string charge = line[current.Item1..current.Item2];

            string posx = line.Substring(current.Item2, 15);
            string posy = line.Substring(current.Item2 + 15, 15);
            string posz = line.Substring(current.Item2 + 30, 15);


            var atom = AtomTable.GetAtomProperties(atomsymbol);

            if (atom is null)
                throw new ApplicationException($"Unknown Atom {atomsymbol}");

            MoleculeAtom retval = new(atom,
                                new PositionVector(StringConversion.ToDouble(posx),
                                                    StringConversion.ToDouble(posy),
                                                        StringConversion.ToDouble(posz)));

            return retval;
        }

        private static int FindFirstSpace(string input, int startPos = 0)
        {
            int retval = startPos;
            for (int c = startPos; c < input.Length; ++c)
            {
                if (input[c] == ' ')
                {
                    retval = c;
                    break;
                }
            }
            return retval;
        }

        private static int FindFirstNoSpace(string input, int startPos = 0)
        {
            int retval = startPos;
            for (int c = startPos; c < input.Length; ++c)
            {
                if (input[c] != ' ')
                {
                    retval = c;
                    break;
                }
            }
            return retval;
        }

        private static Tuple<int, int> FindNthSegment(string input, int n)
        {
            int currentbegin = 0;
            int currentend = 0;
            while (n > 0)
            {
                currentbegin = FindFirstNoSpace(input, currentend);
                currentend = FindFirstSpace(input, currentbegin);
                --n;
            }
            return new Tuple<int, int>(currentbegin, currentend);
        }

    }
}
