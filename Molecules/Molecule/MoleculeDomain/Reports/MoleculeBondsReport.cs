namespace MoleculeDomain.Reports
{
    public sealed class MoleculeBondsReport
    {
        public string MoleculeName { get; set; } = string.Empty;
        public int Atom1Pos { get; set; }
        public int Atom2Pos { get; set; }
        public string BondID { get; set; } = string.Empty;
        public double? Distance { get; set; }
        public double? BondOrder { get; set; }
        public double? OverlapPopulation { get; set; }
        public double? OverlapPopulationHOMO { get; set; }
        public double? OverlapPopulationLUMO { get; set; }
    }
}
