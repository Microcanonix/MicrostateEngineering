using CommonDomain;
using CoreDomain;
using MoleculeDomain.Utilities;

namespace MoleculeDomain.FactoryRequest
{
    public sealed class GmsCalcInputRequest
    {
        public required string MoleculeName { get; set; }

        public required int Charge { get; set; }

        public List<MoleculeAtom> Atoms { get; set; } = [];

        public required CalcBasisSetCodeEnum BasisSet { get; set; }

    }
}
