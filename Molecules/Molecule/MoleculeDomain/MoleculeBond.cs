namespace MoleculeDomain
{
    public class MoleculeBond
    {
        public int Atom1Position { get; set; }

        public int Atom2Position { get; set; }

        public double? Distance { get; set; }

        public MoleculeBondOrder? BondOrder { get; set; }

        public ElectronPopulation? OverlapPopulation { get; set; }
    }
}
