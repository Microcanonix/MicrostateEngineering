using MoleculeDomain.Utilities;

namespace MoleculeDomain
{
    public sealed class ElectronicPotential
    {
        public required PositionVector Position { get; set; }
        public double? Nuclear { get; set; }
        public double? Electronic { get; set; }
        public double? Total { get; set; }

    }
}
