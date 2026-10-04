namespace MoleculeDomain.Reports

{
    public sealed class MoleculeAtomsChargeReport
    {
        /// <summary>
        /// The name of the molecule  to whom the atom belongs
        /// </summary>
        public string MoleculeName { get; set; } = string.Empty;


        /// <summary>
        /// The symbol of the atom followed by its position in the list
        /// </summary>
        public string AtomID { get; set; } = string.Empty;

        /// <summary>
        /// The mulliken charge of the atom
        /// </summary>
        public double? MullikenCharge { get; set; }

        /// <summary>
        /// The Lowdin charge of the atom
        /// </summary>
        public double? LowdinCharge { get; set; }

        /// <summary>
        /// The CHelpGHCharge of the atom
        /// </summary>
        public double? CHelpGHCharge { get; set; }

        /// <summary>
        /// The GeoDiscCharge of thr atom
        /// </summary>
        public double? GeoDiscCharge { get; set; }
    }
}
