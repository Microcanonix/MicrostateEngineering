namespace MoleculeDomain.Reports
{
    public sealed class MoleculeAtomsPopulationReport
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
        /// The MullikenPopulation
        /// </summary>
        public double? MullikenPopulation { get; set; }

        /// <summary>
        /// The HOMO population refers to Lewis Base 
        /// </summary>
        public double? MullikenPopulationHOMO { get; set; }

        /// <summary>
        /// The LUMO popultion refers to the Lewis Acid
        /// </summary>
        public double? MullikenPopulationLUMO { get; set; }

        /// <summary>
        /// The LowdinPopulation
        /// </summary>
        public double? LowdinPopulation { get; set; }

        /// <summary>
        /// The HOMO population refers to Lewis Base 
        /// </summary>
        public double? LowdinPopulationHOMO { get; set; }

        /// <summary>
        /// The LUMO popultion refers to the Lewis Acid
        /// </summary>
        public double? LowdinPopulationLUMO { get; set; }
    }
}
