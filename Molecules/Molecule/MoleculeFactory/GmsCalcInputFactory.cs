using CommonDomain;
using IMoleculeFactory;
using MoleculeDomain.FactoryRequest;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Utilities;
using System.Text;

namespace MoleculeFactory
{
    public sealed class GmsCalcInputFactory : IGmsCalcInputFactory
    {
        
        private MoleculeFileGmsInput Init(GmsCalcInputFactoryRequest request, StepType stepType, string additionalSymbol = "")
        {
            return new MoleculeFileGmsInput()
            {
                Name = new MoleculeFileName(request.MoleculeName, request.Charge, request.BasisSet, stepType, additionalSymbol)
            };
        }

        public MoleculeFileGmsInput BuildCHelpGChargeInput(GmsCalcInputFactoryRequest request)
        {
            var result = Init(request, StepType.charge_chelpg);

            StringBuilder retval = new();
            var basisSetInput = CalcBasisSetTable.GetCalcBasisSetGmsInput(request.BasisSet);
            retval.AppendLine($" {basisSetInput}");
            retval.AppendLine($" $CONTRL SCFTYP=RHF DFTTYP=B3LYP MAXIT=60 MULT=1 ICHARG={request.Charge} $END");
            retval.AppendLine(" $SYSTEM MEMDDI=1000 MWORDS=30 $END");
            retval.AppendLine(" $SCF DIRSCF=.TRUE. $END");
            retval.AppendLine(" $ELPOT  IEPOT=1 WHERE=PDC $END");
            retval.AppendLine(" $PDC PTSEL=CHELPG CONSTR=CHARGE $END");
            retval.AppendLine(" $DATA");
            retval.AppendLine();
            retval.AppendLine("C1");
            foreach (var moleculeAtom in request.Atoms)
            {
                retval.AppendLine($"{moleculeAtom.Atom.Name} {moleculeAtom.Atom.AtomNumber:0.0} {moleculeAtom.Pos.PosX} {moleculeAtom.Pos.PosY} {moleculeAtom.Pos.PosZ}".Replace(",", "."));
            }
            retval.AppendLine(" $END");
            result.Content = retval.ToString();
            return result;
        }

        public MoleculeFileGmsInput BuildFukuiHOMOInput(GmsCalcInputFactoryRequest request)
        {
            var result = Init(request, StepType.fukui_calculation, AdditionalSymbols.Plus);
            StringBuilder retval = new();
            var basisSetInput = CalcBasisSetTable.GetCalcBasisSetGmsInput(request.BasisSet);
            retval.AppendLine($" {basisSetInput}");
            retval.AppendLine($" $CONTRL SCFTYP=UHF MAXIT=60 MULT=2 ICHARG={request.Charge + 1} $END");
            retval.AppendLine($" $SYSTEM MEMDDI=1000 MWORDS=30 $END");
            retval.AppendLine($" $SCF DIRSCF=.TRUE. $END");
            retval.AppendLine(" $STATPT OPTTOL=0.0001 NSTEP=999 $END");
            retval.AppendLine(" $DATA");
            retval.AppendLine();
            retval.AppendLine("C1");
            foreach (var moleculeAtom in request.Atoms)
            {
                retval.AppendLine($"{moleculeAtom.Atom.Name} {moleculeAtom.Atom.AtomNumber:0.0} {moleculeAtom.Pos.PosX} {moleculeAtom.Pos.PosY} {moleculeAtom.Pos.PosZ}".Replace(",", "."));
            }
            retval.AppendLine(" $END");
            result.Content = retval.ToString();
            return result;
        }

        public MoleculeFileGmsInput BuildFukuiLUMOInput(GmsCalcInputFactoryRequest request)
        {
            var result = Init(request, StepType.fukui_calculation, AdditionalSymbols.Minus);
            StringBuilder retval = new();
            var basisSetInput = CalcBasisSetTable.GetCalcBasisSetGmsInput(request.BasisSet);
            retval.AppendLine($" {basisSetInput}");
            retval.AppendLine($" $CONTRL SCFTYP=UHF MAXIT=60 MULT=2 ICHARG={request.Charge - 1} $END");
            retval.AppendLine($" $SYSTEM MEMDDI=1000 MWORDS=30 $END");
            retval.AppendLine($" $SCF DIRSCF=.TRUE. $END");
            retval.AppendLine(" $STATPT OPTTOL=0.0001 NSTEP=999 $END");
            retval.AppendLine(" $DATA");
            retval.AppendLine();
            retval.AppendLine("C1");
            foreach (var moleculeAtom in request.Atoms)
            {
                retval.AppendLine($"{moleculeAtom.Atom.Name} {moleculeAtom.Atom.AtomNumber:0.0} {moleculeAtom.Pos.PosX} {moleculeAtom.Pos.PosY} {moleculeAtom.Pos.PosZ}".Replace(",", "."));
            }
            retval.AppendLine(" $END");
            result.Content = retval.ToString();
            return result;
        }

        public MoleculeFileGmsInput BuildGeoDiskChargeInput(GmsCalcInputFactoryRequest request)
        {
            var result = Init(request, StepType.charge_geodisk);
            StringBuilder retval = new();
            var basisSetInput = CalcBasisSetTable.GetCalcBasisSetGmsInput(request.BasisSet);
            retval.AppendLine($" {basisSetInput}");
            retval.AppendLine($" $CONTRL SCFTYP=RHF DFTTYP=B3LYP MAXIT=60 MULT=1 ICHARG={request.Charge} $END");
            retval.AppendLine(" $SYSTEM MEMDDI=1000 MWORDS=30 $END");
            retval.AppendLine(" $SCF DIRSCF=.TRUE. $END");
            retval.AppendLine(" $ELPOT  IEPOT=1 WHERE=PDC $END");
            retval.AppendLine(" $PDC PTSEL=GEODESIC CONSTR=CHARGE $END");
            retval.AppendLine(" $DATA");
            retval.AppendLine();
            retval.AppendLine("C1");
            foreach (var moleculeAtom in request.Atoms)
            {
                retval.AppendLine($"{moleculeAtom.Atom.Name} {moleculeAtom.Atom.AtomNumber:0.0} {moleculeAtom.Pos.PosX} {moleculeAtom.Pos.PosY} {moleculeAtom.Pos.PosZ}".Replace(",", "."));
            }
            retval.AppendLine(" $END");
            result.Content = retval.ToString();
            return result;
        }

        public MoleculeFileGmsInput BuildGeoOptGmsInput(GmsCalcInputFactoryRequest request)
        {
            var result = Init(request, StepType.geometry_optimization);
            StringBuilder retval = new();
            var basisSetInout = CalcBasisSetTable.GetCalcBasisSetGmsInput(request.BasisSet);
            retval.AppendLine($" {basisSetInout}");
            retval.AppendLine($" $CONTRL SCFTYP=RHF RUNTYP=OPTIMIZE DFTTYP=B3LYP MAXIT=60 MULT=1 ICHARG={request.Charge} $END");
            retval.AppendLine(" $SYSTEM MEMDDI=1000 MWORDS=30 $END");
            retval.AppendLine(" $STATPT NSTEP=999 $END");
            retval.AppendLine($" $SCF DIRSCF=.TRUE. $END");
            retval.AppendLine(" $DATA");
            retval.AppendLine();
            retval.AppendLine("C1");
            foreach (var moleculeAtom in request.Atoms)
            {
                retval.AppendLine($"{moleculeAtom.Atom.Name} {moleculeAtom.Atom.AtomNumber:0.0} {moleculeAtom.Pos.PosX} {moleculeAtom.Pos.PosY} {moleculeAtom.Pos.PosZ}".Replace(",", "."));
            }
            retval.AppendLine(" $END");
            result.Content = retval.ToString();
            return result;
        }

        public MoleculeFileGmsInput BuildFukuiNeutralInput(GmsCalcInputFactoryRequest request)
        {
            var result = Init(request, StepType.fukui_calculation, AdditionalSymbols.Neutral);
            StringBuilder retval = new();
            var basisSetInput = CalcBasisSetTable.GetCalcBasisSetGmsInput(request.BasisSet);
            retval.AppendLine($" {basisSetInput}");
            retval.AppendLine($" $CONTRL SCFTYP=RHF MAXIT=60 MULT=1 ICHARG={request.Charge} $END");
            retval.AppendLine($" $SYSTEM MEMDDI=1000 MWORDS=30 $END");
            retval.AppendLine($" $SCF DIRSCF=.TRUE. $END");
            retval.AppendLine(" $DATA");
            retval.AppendLine();
            retval.AppendLine("C1");
            foreach (var moleculeAtom in request.Atoms)
            {
                retval.AppendLine($"{moleculeAtom.Atom.Name} {moleculeAtom.Atom.AtomNumber:0.0} {moleculeAtom.Pos.PosX} {moleculeAtom.Pos.PosY} {moleculeAtom.Pos.PosZ}".Replace(",", "."));
            }
            retval.AppendLine(" $END");
            result.Content = retval.ToString();
            return result;
        }

        public MoleculeFileGmsInput BuildElectronicStructureInput(GmsCalcInputFactoryRequest request)
        {
            var result = Init(request, StepType.electronic_structure);
            StringBuilder retval = new();
            var basisSetInput = CalcBasisSetTable.GetCalcBasisSetGmsInput(request.BasisSet);
            retval.AppendLine($" {basisSetInput}");
            retval.AppendLine($" $CONTRL SCFTYP=RHF MAXIT=60 MULT=1 ICHARG={request.Charge} $END");
            retval.AppendLine($" $SYSTEM MEMDDI=1000 MWORDS=30 $END");
            retval.AppendLine($" $SCF DIRSCF=.TRUE. $END");
            retval.AppendLine(" $DATA");
            retval.AppendLine();
            retval.AppendLine("C1");
            foreach (var moleculeAtom in request.Atoms)
            {
                retval.AppendLine($"{moleculeAtom.Atom.Name} {moleculeAtom.Atom.AtomNumber:0.0} {moleculeAtom.Pos.PosX} {moleculeAtom.Pos.PosY} {moleculeAtom.Pos.PosZ}".Replace(",", "."));
            }
            retval.AppendLine(" $END");
            result.Content = retval.ToString();
            return result;
        }
    }
}
