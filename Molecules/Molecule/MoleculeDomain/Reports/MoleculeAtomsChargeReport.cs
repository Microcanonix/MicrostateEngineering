namespace MoleculeDomain.Reports

{
    public sealed class MoleculeAtomsChargeReport
    {
        public string MoleculeName { get; set; } = string.Empty;

        public string AtomID { get; set; } = string.Empty;

        public double? MullikenCharge { get; set; }

        public double? LowdinCharge { get; set; }

        public double? CHelpGHCharge { get; set; }

        public double? GeoDiscCharge { get; set; }
    }
}
