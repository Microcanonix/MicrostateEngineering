using MoleculeDomain;
using MoleculeDomain.Utilities;
using MoleculeFactory.Conversion;

namespace MoleculeFactory.Parsers
{
    public static class ChargeParser
    {
        private const string StartTag = "          ELECTROSTATIC POTENTIAL";
        private const string StartChargedTag = " NET CHARGES:";
        private const string EndChargeTag = " -------------------------------------";
        private const string StartElpotTag = " NUMBER OF POINTS SELECTED FOR FITTING";
        private const string GeoDiscTag = "PTSEL=GEODESIC";
        private static readonly string[] Whitespace = [" "];

        public static void Parse(List<string> fileLines, Molecule molecule)
        {
            bool overallstart = false;
            bool startCharge = false;
            bool startElpot = false;
            bool isGeoDisc = false;
            int currentAtomPos = 1;
            for (int c = 0; c < fileLines.Count; ++c)
            {
                var line = fileLines[c];

                if (line.Contains(StartTag))
                {
                    overallstart = true;
                }

                if (line.Contains(GeoDiscTag))
                {
                    isGeoDisc = true;
                }

                if (overallstart && line.Contains(StartChargedTag))
                {
                    startCharge = true;
                    c += 3;
                    continue;
                }



                if (overallstart && line.StartsWith(StartElpotTag))
                {
                    startElpot = true;
                    continue;
                }

                if (startElpot)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        startElpot = false;
                        continue;
                    }

                    var data = line.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
                    if (data.Length > 6)
                    {
                        ElectronicPotential item = new()
                        {
                            Position = new PositionVector(StringConversion.ToDouble(data[1]), StringConversion.ToDouble(data[2]), StringConversion.ToDouble(data[3])),
                            Electronic = StringConversion.ToDouble(data[4]),
                            Nuclear = StringConversion.ToDouble(data[5]),
                            Total = StringConversion.ToDouble(data[6])
                        };

                        //if ( isGeoDisc)
                        //{
                        //    if (!molecule.GeoDiskElpot.Any(x => x.Position == item.Position))
                        //    {
                        //        molecule.GeoDiskElpot.Add(item);
                        //    }
                        //}
                        //else
                        //{
                        //    if (!molecule.ChelpGElpot.Any(x => x.Position == item.Position))
                        //    {
                        //        molecule.ChelpGElpot.Add(item);
                        //    }
                        //} 
                    }
                }

                if (startCharge)
                {
                    if (line.Contains(EndChargeTag))
                    {
                        return;
                    }

                    var data = line.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
                    if (data.Length > 2)
                    {
                        string symbol = data[0];
                        double charge = StringConversion.ToDouble(data[1].Trim());
                        var atom = molecule.Atoms.Find(i => i.PositionInMolecule == currentAtomPos 
                                                        && 
                                                       string.Compare(i.Atom.Name, symbol, true) == 0);
                        if (atom != null)
                        {
                            atom.Charge ??= new Charge();
                            if (isGeoDisc)
                            {
                                atom.Charge.GeoDiscCharge = charge;
                            }
                            else
                            {
                                atom.Charge.CHelpGCharge = charge;
                            }

                        }
                        ++currentAtomPos;
                    }
                }
            }
        }


    }
}
