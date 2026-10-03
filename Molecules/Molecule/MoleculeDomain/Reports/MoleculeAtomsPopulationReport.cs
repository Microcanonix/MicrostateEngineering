namespace MoleculeDomain.Reports
{
    public sealed class MoleculeAtomsPopulationReport
    {
        public string MoleculeName { get; set; } = string.Empty;

        public string AtomID { get; set; } = string.Empty;

        public double? MullikenPopulation { get; set; }

        public double? MullikenPopulationHOMO { get; set; }

        public double? MullikenPopulationLUMO { get; set; }

        public double? LowdinPopulation { get; set; }

        public double? LowdinPopulationHOMO { get; set; }

        public double? LowdinPopulationLUMO { get; set; }
    }
}
