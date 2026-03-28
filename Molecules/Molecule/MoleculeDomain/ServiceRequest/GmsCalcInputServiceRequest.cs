using MoleculeDomain.Utilities;

namespace MoleculeDomain.ServiceRequest
{
    public sealed class GmsCalcInputServiceRequest
    {
        public required string GmsInputFileDirectory { get; set; }

        public required string MoleculeFileDirectory { get; set; }

        public required string MoleculeName { get; set; }

        public required int Charge { get; set; }

        public required CalcBasisSetCodeEnum BasisSet { get; set; }
    }
}
