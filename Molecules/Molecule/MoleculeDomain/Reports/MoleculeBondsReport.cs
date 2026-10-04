namespace MoleculeDomain.Reports
{
    public sealed class MoleculeBondsReport
    {
        /// <summary>
        /// The name of the molecule  to whom the atom belongs
        /// </summary>
        public string MoleculeName { get; set; } = string.Empty;

        /// <summary>
        /// The position of the first atom in the list of atoms
        /// </summary>
        public int Atom1Pos { get; set; }

        /// <summary>
        /// The position of the second atom in the list of atoms
        /// </summary>
        public int Atom2Pos { get; set; }

        /// <summary>
        /// The bond identifyer
        /// </summary>
        public string BondID { get; set; } = string.Empty;

        /// <summary>
        /// The bond distance
        /// </summary>
        public double? Distance { get; set; }

        /// <summary>
        /// The bondOrder
        /// </summary>
        public double? BondOrder { get; set; }

        /// <summary>
        /// Mulliken atomic overlap population
        /// </summary>
        public double? OverlapPopulation { get; set; }

        /// <summary>
        /// Mulliken atomic overlap population of the HOMO, the lewis base 
        /// </summary>
        public double? OverlapPopulationHOMO { get; set; }

        /// <summary>
        /// Mulliken atomic overlap population of the LUMO, the lewis acid
        /// </summary>
        public double? OverlapPopulationLUMO { get; set; }
    }
}
