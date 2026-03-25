using IMoleculeFactory;
using MoleculeDomain.FactoryRequest;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Utilities;
using System.Text;

namespace MoleculeFactory
{
    public sealed class GmsCalcInputFactory : IGmsCalcInputFactory
    {
        
        private MoleculeFileGmsInput Init(GmsCalcInputRequest request)
        {
            return new MoleculeFileGmsInput()
            {
                Name = $"{request.MoleculeName}_{request.Charge}_{request.BasisSet}_{request.StepType}"
            };
        }

        public MoleculeFileGmsInput BuildCHelpGChargeInput(GmsCalcInputRequest request)
        {
            var result = Init(request);

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

        public MoleculeFileGmsInput BuildFukuiHOMOInput(GmsCalcInputRequest request)
        {
            var result = Init(request);
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

        public MoleculeFileGmsInput BuildFukuiLUMOInput(GmsCalcInputRequest request)
        {
            var result = Init(request);
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

        public MoleculeFileGmsInput BuildGeoDiskChargeInput(GmsCalcInputRequest request)
        {
            var result = Init(request);
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

        public MoleculeFileGmsInput BuildGeoOptGmsInput(GmsCalcInputRequest request)
        {
            var result = Init(request);
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

        public MoleculeFileGmsInput BuildNeutralInput(GmsCalcInputRequest request)
        {
            var result = Init(request);
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
