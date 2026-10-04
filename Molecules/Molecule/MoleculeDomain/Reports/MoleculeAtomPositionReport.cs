namespace MoleculeDomain.Reports

{
    public sealed class MoleculeAtomPositionReport
    {
        /// <summary>
        /// The name of the molecule  to whom the atom belongs
        /// </summary>
        public string MoleculeName { get; set; } = string.Empty;

        /// <summary>
        /// The position of the Atom in the list
        /// </summary>
        public int AtomPosition { get; set; }

        /// <summary>
        /// The atomic symbol
        /// </summary>
        public string AtomSymbol { get; set; } = string.Empty;

        /// <summary>
        /// X coordinate of the atom
        /// </summary>
        public double? PosX { get; set; }

        /// <summary>
        /// Y coordinate of the atom
        /// </summary>
        public double? PosY { get; set; }

        /// <summary>
        /// Z coordinate of the atom
        /// </summary>
        public double? PosZ { get; set; }
    }
}
