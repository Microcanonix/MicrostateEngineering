namespace MoleculeDomain.Reports
{
    public class MoleculeBondsReport
    {
        public string MoleculeName { get; set; } = "";

        public int Atom1Pos { get; set; }

        public int Atom2Pos { get; set; }

        public string BondID { get; set; } = "";
        public double? Distance { get; set; }
        public double? BondOrder { get; set; }
        public double? OverlapPopulation { get; set; }
        public double? OverlapPopulationHOMO { get; set; }
        public double? OverlapPopulationLUMO { get; set; }
    }
}
