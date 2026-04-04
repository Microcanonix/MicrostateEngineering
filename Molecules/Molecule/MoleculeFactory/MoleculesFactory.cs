using CoreDomain;
using CoreFactories.Parsers;
using IMoleculeFactory;
using IUtilitiesServices;
using MoleculeDomain;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Utilities;
using MoleculeFactory.Conversion;
using MoleculeFactory.Parsers;

namespace MoleculeFactory
{
    public sealed class MoleculesFactory : IMoleculesFactory
    {
        private readonly IJsonParser<Molecule> _jsonParser;

        public MoleculesFactory(IJsonParser<Molecule> jsonParser)
        {
            _jsonParser = jsonParser;
        }

        public Molecule BuildMolecule(MoleculeFileMoleculeData moleculeData)
        {
            return _jsonParser.Parse(moleculeData.Content);
        }

        public Molecule BuildMolecule(MoleculeFileXyz moleculeFileXyz, string name, int charge)
        {
            var result = XyzConversion.ParseXyz(moleculeFileXyz.Content);
            Molecule molecule = new Molecule(name, charge);
            int counter = 1;
            foreach(var item in result)
            {
                Atom? current = AtomTable.GetAtomProperties(item.Symbol);               
                if ( current is null)
                {
                    throw new ApplicationException($"Unknown atom symbol {item.Symbol}");
                }
                MoleculeAtom newAtom = new MoleculeAtom(current,new PositionVector(item.PosX, item.PosY, item.PosZ) );
                newAtom.PositionInMolecule = counter++;
                molecule.Atoms.Add(newAtom);
            }
            return molecule;
        }

        public bool TryCompleteMolecule(Molecule molecule, MoleculeFileGmsOutput moleculeFileGmsOutput, OutputFileType fileType)
        {
            if (string.IsNullOrEmpty(moleculeFileGmsOutput.Content)) return false;
            List<string> fileLines = moleculeFileGmsOutput.GetLines();
            switch (fileType)
            {
                case OutputFileType.geometry_optimization:
                    if (GmsCalcValidityParser.TryParse(fileLines, molecule))
                    {
                        GeoOptParser.Parse(fileLines, molecule);
                        GeoOptDftEnergyParser.Parse(fileLines, molecule);
                        return true;
                    }
                    return false;
                case OutputFileType.electronic_structure:
                    if (GmsCalcValidityParser.TryParse(fileLines, molecule))
                    {
                        NeutralPopulationAnalysisParser.GetPopulation(fileLines, molecule);
                        molecule.HFEnergy = FukuiEnergyNeutralParser.GetEnergy(fileLines);
                        return true;
                    }
                    return false;
                case OutputFileType.fukui_calculation_neutral:
                    if (GmsCalcValidityParser.TryParse(fileLines, molecule))
                    {
                        NeutralPopulationAnalysisParser.GetPopulation(fileLines, molecule);
                        molecule.HFEnergy = FukuiEnergyNeutralParser.GetEnergy(fileLines);
                        return true;
                    }
                    return false;
                case OutputFileType.fukui_calculation_lumo:
                    if (GmsCalcValidityParser.TryParse(fileLines, molecule))
                    {
                        LewisLUMOPopulationAnalysisParser.GetPopulation(fileLines, molecule);
                        molecule.HFEnergyLUMO = FukuiEnergyLewisLUMOParser.GetEnergy(fileLines);
                        return true;
                    }
                    return false;
                case OutputFileType.fukui_calculation_homo:
                    if (GmsCalcValidityParser.TryParse(fileLines, molecule))
                    {
                        LewisHOMOPopulationAnalysisParser.GetPopulation(fileLines, molecule);
                        molecule.HFEnergyHOMO = FukuiEnergyLewisHOMOParser.GetEnergy(fileLines);
                        return true;
                    }
                    return false;
                case OutputFileType.charge_geodisk:
                    if (GmsCalcValidityParser.TryParse(fileLines, molecule))
                    {
                        ChargeParser.Parse(fileLines, molecule);
                        return true;
                    }
                    return false;
                case OutputFileType.charge_chelpg:
                    if (GmsCalcValidityParser.TryParse(fileLines, molecule))
                    {
                        ChargeParser.Parse(fileLines, molecule);
                        return true;
                    }
                    return false;
                default:
                    break;
            }
            return false;
        }

        public MoleculeFileMoleculeData BuildMoleculeDataFile(Molecule molecule)
        {
            return new MoleculeFileMoleculeData()
            {
                Name = new MoleculeFileName(molecule.Name),
                Content = _jsonParser.Serialize(molecule)
            };
        }

        public MoleculeFileXyz BuildMoleculeXyzFile(Molecule molecule)
        {
            return new MoleculeFileXyz()
            {
                Name = new MoleculeFileName(molecule.Name),
                Content = GetXyzFileData(molecule)
            };
        }

        public static string GetXyzFileData(Molecule molecule)
        {
            return XyzConversion.SerializeXyz(molecule.Atoms.ConvertAll(x => 
                                                    new AtomPosition(x.Atom.Symbol.ToString(),
                                                                        x.Pos.PosX,
                                                                        x.Pos.PosY,
                                                                        x.Pos.PosZ)));
        }


    }
}
