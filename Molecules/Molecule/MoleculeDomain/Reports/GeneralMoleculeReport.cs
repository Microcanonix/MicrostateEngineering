namespace MoleculeDomain.Reports
{
    public sealed class GeneralMoleculeReport
    {
        public string MoleculeName { get; set; } = string.Empty;

        public string AtomID { get; set; } = string.Empty;

        public double? CHelpGCharge { get; set; }

        public double? MullNeutral { get; set; }

        public string Configuration { get; set; } = string.Empty;

        public double? MullLewisAcid { get; set; }

        public string ConfigurationLewisAcid { get; set; } = string.Empty;

        public double? MullLewisBase { get; set; }

        public string ConfigurationLewisBase { get; set; } = string.Empty;

        public List<ConfigurationReportItem> ConfigurationItems { get; set; } = new List<ConfigurationReportItem>();

        public List<ConfigurationReportItem> ConfigurationItemsLewisBase { get; set; } = new List<ConfigurationReportItem>();

        public List<ConfigurationReportItem> ConfigurationItemsLewisAcid { get; set; } = new List<ConfigurationReportItem>();

    }
}
