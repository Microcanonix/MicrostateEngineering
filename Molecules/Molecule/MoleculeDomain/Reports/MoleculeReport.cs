namespace MoleculeDomain.Reports
{
    public sealed class MoleculeReport
    {
        public List<GeneralMoleculeReport> GeneralReport { get; set; } = new();

        public List<MoleculeAtomPositionReport> AtomPositionsReport { get; set; } = new();

        public List<MoleculeAtomOrbitalReport> AtomOrbitalReport { get; set; } = new();

        public List<MoleculeAtomsChargeReport> AtomChargeReport { get; set; } = new();

        public List<MoleculeAtomsPopulationReport> AtomPopulationReport { get; set; } = new();

        public List<MoleculeBondsReport> MoleculeBondsReport { get; set; } = new();
    }
}
