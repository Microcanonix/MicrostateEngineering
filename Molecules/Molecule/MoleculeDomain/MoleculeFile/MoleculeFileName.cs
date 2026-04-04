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
            throw new NotImplementedException();
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

            fileName.Append(AdditionalSymbol);

            return fileName.ToString();
        }

    }
}
