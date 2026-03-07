using CoreDomain;
using IMoleculeFactory;
using IUtilitiesServices;
using MoleculeDomain;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Utilities;
using MoleculeFactory.Conversion;

namespace MoleculeFactory
{
    public sealed class BuildMoleculeFactory : IBuildMoleculeFactory
    {
        private readonly IJsonParser<Molecule> _jsonParser;

        public BuildMoleculeFactory(IJsonParser<Molecule> jsonParser)
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

        public Molecule CompleteMolecule(Molecule molecule, MoleculeFileGmsOutput moleculeFileGmsOutput)
        {
            if (string.IsNullOrEmpty(moleculeFileGmsOutput.Content)) return molecule;
            List<string> fileLines = moleculeFileGmsOutput.Lines;
            //switch (moleculeFileGmsOutput.StepType)
            //{        
            //    case CommonDomain.Workflow.Enums.StepTypeEnum.geometry_optimization:
            //        if (GmsCalcValidityParser.TryParse (fileLines, molecule))
            //        {
            //            GeoOptParser.Parse(fileLines, molecule);
            //            GeoOptDftEnergyParser.Parse(fileLines, molecule);
            //        }
            //        break;
                
            //    case CommonDomain.Workflow.Enums.StepTypeEnum.electronic_structure:
            //        if (GmsCalcValidityParser.TryParse(fileLines, molecule))
            //        {
            //            NeutralPopulationAnalysisParser.GetPopulation(fileLines, molecule);
            //            molecule.HFEnergy = FukuiEnergyNeutralParser.GetEnergy(fileLines);
            //        }
            //        break;
               
            //    case CommonDomain.Workflow.Enums.StepTypeEnum.neutral:
            //        if (GmsCalcValidityParser.TryParse(fileLines, molecule))
            //        {
            //            NeutralPopulationAnalysisParser.GetPopulation(fileLines, molecule);
            //            molecule.HFEnergy = FukuiEnergyNeutralParser.GetEnergy(fileLines);
            //        }
            //        break;
            //    case CommonDomain.Workflow.Enums.StepTypeEnum.plus:
            //        if (GmsCalcValidityParser.TryParse(fileLines, molecule))
            //        {
            //            LewisLUMOPopulationAnalysisParser.GetPopulation(fileLines, molecule);
            //            molecule.HFEnergyLUMO = FukuiEnergyLewisLUMOParser.GetEnergy(fileLines);
            //        }
            //        break;
            //    case CommonDomain.Workflow.Enums.StepTypeEnum.minus:
            //        if (GmsCalcValidityParser.TryParse(fileLines, molecule))
            //        {
            //            LewisHOMOPopulationAnalysisParser.GetPopulation(fileLines, molecule);
            //            molecule.HFEnergyHOMO = FukuiEnergyLewisHOMOParser.GetEnergy(fileLines);
            //        }
            //        break;
            //    case CommonDomain.Workflow.Enums.StepTypeEnum.geodisk:
            //        if (GmsCalcValidityParser.TryParse(fileLines, molecule))
            //        {
            //            ChargeParser.Parse(fileLines, molecule);
            //        }
            //        break;
            //    case CommonDomain.Workflow.Enums.StepTypeEnum.chelpg:
            //        if (GmsCalcValidityParser.TryParse(fileLines, molecule))
            //        {
            //            ChargeParser.Parse(fileLines, molecule);
            //        }
            //        break;
            //    default:
            //        break;
            //}
            return molecule;
        }

        public MoleculeFileMoleculeData BuildMoleculeDataFile(Molecule molecule)
        {
            return new MoleculeFileMoleculeData()
            {
                Name = molecule.Name,
                Content = _jsonParser.Serialize(molecule)
            };
        }

        public MoleculeFileXyz BuildMoleculeXyzFile(Molecule molecule)
        {
            return new MoleculeFileXyz()
            {
                Name = molecule.Name,
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
