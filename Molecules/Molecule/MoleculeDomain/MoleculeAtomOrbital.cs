namespace MoleculeDomain
{
    public class MoleculeAtomOrbital
    {
        public int Position { get; set; }
        public string? Symbol { get; set; }

        public ElectronPopulation? MullikenPopulation { get; set; }

        public ElectronPopulation? LowdinPopulation { get; set; }

    }
}
