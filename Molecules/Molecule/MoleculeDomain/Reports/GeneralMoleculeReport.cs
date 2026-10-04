namespace MoleculeDomain.Reports
{
    public sealed class GeneralMoleculeReport
    {
        /// <summary>
        /// The name of the molecule
        /// </summary>
        public string MoleculeName { get; set; } = string.Empty;

        /// <summary>
        /// The symbol of the atom followed by the position in the list 
        /// </summary>
        public string AtomID { get; set; } = string.Empty;

        /// <summary>
        /// The CHelpGCharge on the atom
        /// </summary>
        public double? CHelpGCharge { get; set; }

        /// <summary>
        /// The mulliken population of the atom
        /// </summary>
        public double? MullNeutral { get; set; }

        /// <summary>
        /// The partitioning of the electron density across the orbitals of this atom ( as fraction of the total atom population )
        /// </summary>
        public string Configuration { get; set; } = string.Empty;

        /// <summary>
        /// The Lumo population of the atom
        /// </summary>
        public double? MullLewisAcid { get; set; }

        /// <summary>
        /// The partitioning of the electron LUMO density across the orbitals of this atom ( as fraction of the lumo atom population )
        /// </summary>
        public string ConfigurationLewisAcid { get; set; } = string.Empty;

        /// <summary>
        /// The homo popultion of the atom
        /// </summary>
        public double? MullLewisBase { get; set; }

        /// <summary>
        /// The partitioning of the electron HOMO density across the orbitals of this atom ( as fraction of the homo atom population )
        /// </summary>
        public string ConfigurationLewisBase { get; set; } = string.Empty;

        public List<ConfigurationReportItem> ConfigurationItems { get; set; } = new List<ConfigurationReportItem>();

        public List<ConfigurationReportItem> ConfigurationItemsLewisBase { get; set; } = new List<ConfigurationReportItem>();

        public List<ConfigurationReportItem> ConfigurationItemsLewisAcid { get; set; } = new List<ConfigurationReportItem>();

    }
}
