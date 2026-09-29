namespace MoleculeDomain.Reports

{
    public class MoleculeAtomsChargeReport
    {
        public string MoleculeName { get; set; } = "";

        public string AtomID { get; set; } = "";

        public double? MullikenCharge { get; set; }

        public double? LowdinCharge { get; set; }

        public double? CHelpGHCharge { get; set; }

        public double? GeoDiscCharge { get; set; }
    }
}
