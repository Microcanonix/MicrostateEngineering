namespace MoleculeDomain.Reports
{
    public class MoleculeAtomsPopulationReport
    {
        public string MoleculeName { get; set; } = "";

        public string AtomID { get; set; } = "";

        public double? MullikenPopulation { get; set; }

        public double? MullikenPopulationHOMO { get; set; }

        public double? MullikenPopulationLUMO { get; set; }

        public double? LowdinPopulation { get; set; }

        public double? LowdinPopulationHOMO { get; set; }

        public double? LowdinPopulationLUMO { get; set; }
    }
}
