using MoleculeDomain.Utilities;

namespace MoleculeDomain.ServiceRequest
{
    public sealed class GmsCalcCompleteMoleculeRequest
    {
        public required string MoleculeDataFileDirectory { get; set; }

        public required string GmsOutputFileDirectory { get; set; }

        public required string MoleculeName { get; set; }

        public required int Charge { get; set; }

        public required CalcBasisSetCodeEnum BasisSet { get; set; }
    }

}
