using CommonDomain;
using MoleculeDomain.Utilities;
using System.Text;

namespace MoleculeDomain.MoleculeFile
{
    public sealed class MoleculeFileName
    {

        public MoleculeFileName(string moleculeName, int charge, CalcBasisSetCodeEnum basisSet, StepType stepType )
        {
            MoleculeName = moleculeName;
            Charge = charge;
            BasisSet = basisSet;
            StepType = stepType;
            AdditionalSymbol = string.Empty;
        }

        public MoleculeFileName(string moleculeName, int charge, CalcBasisSetCodeEnum basisSet, StepType stepType,  string additionalSymbol)
        {
            MoleculeName = moleculeName;
            Charge = charge;
            BasisSet = basisSet;
            StepType = stepType;
            AdditionalSymbol = additionalSymbol;
        }

        public MoleculeFileName(string moleculeName)
        {
            MoleculeName = moleculeName;
            AdditionalSymbol = string.Empty;
        }


        public static MoleculeFileName Parse(string fileName)
        {
            var segments = fileName.Split('_', StringSplitOptions.RemoveEmptyEntries);
            if ( segments.Length == 1)
            {
                return new MoleculeFileName(segments[0]);
            }
            if ( segments.Length == 4 )
            {
                string moleculeName = segments[0];
                int charge = int.TryParse(segments[1], out int lccharge) ? lccharge : 0;
                CalcBasisSetCodeEnum basisSet = Enum.TryParse<CalcBasisSetCodeEnum>(segments[2], out var lcbasisSet) ? lcbasisSet : CalcBasisSetCodeEnum.Dummy;
                StepType stepType = Enum.TryParse<StepType>(segments[3], out var lcStepType) ? lcStepType : CommonDomain.StepType.dummy;
                return new MoleculeFileName(moleculeName, charge, basisSet, stepType);
            }
            if (segments.Length == 5)
            {
                string moleculeName = segments[0];
                int charge = int.TryParse(segments[1], out int lccharge) ? lccharge : 0;
                CalcBasisSetCodeEnum basisSet = Enum.TryParse<CalcBasisSetCodeEnum>(segments[2], out var lcbasisSet) ? lcbasisSet : CalcBasisSetCodeEnum.Dummy;
                StepType stepType = Enum.TryParse<StepType>(segments[3], out var lcStepType) ? lcStepType : CommonDomain.StepType.dummy;
                return new MoleculeFileName(moleculeName, charge, basisSet, stepType, segments[4]);
            }
            throw new ArgumentException(fileName);
        }

        public string MoleculeName { get; set; }

        public int? Charge { get; set; }

        public CalcBasisSetCodeEnum? BasisSet { get; set; }

        public StepType? StepType { get; set; }

        public string AdditionalSymbol { get; set; }


        public override string ToString()
        {
            StringBuilder fileName = new StringBuilder(MoleculeName);

            if ( Charge.HasValue)
            {
                fileName.Append($"_{Charge}");
            }

            if ( BasisSet.HasValue)
            {
                fileName.Append($"_{BasisSet}");
            }

            if ( StepType.HasValue)
            {
                fileName.Append($"_{StepType}");
            }

            if ( !string.IsNullOrEmpty(AdditionalSymbol))
            {
                fileName.Append("_" + AdditionalSymbol);
            }
           

            return fileName.ToString();
        }

    }
}
