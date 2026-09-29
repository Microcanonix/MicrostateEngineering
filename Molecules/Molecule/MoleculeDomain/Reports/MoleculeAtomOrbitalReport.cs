using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace MoleculeDomain.Reports
{
    public class MoleculeAtomOrbitalReport
    {
        public string MoleculeName { get; set; } = "";

        public string AtomID { get; set; } = "";

        public double? MullikenPopulation { get; set; }

        public string ElectronConfiguration
        {
            get
            {
                StringBuilder sb = new();
                OrbitalReport.ForEach(orbital => sb.Append($"{orbital.OrbitalSymbol}({orbital.PopulationFraction?.ToString("0.00",CultureInfo.InvariantCulture)})"));
                return sb.ToString();
            }
        }

        public string ElectronConfigurationHOMO
        {
            get
            {
                StringBuilder sb = new();
                OrbitalReport.ForEach(orbital => sb.Append($"{orbital.OrbitalSymbol}({orbital.PopulationFractionHOMO?.ToString("0.00", CultureInfo.InvariantCulture)})"));
                return sb.ToString();
            }
        }

        public string ElectronConfigurationLUMO
        {
            get
            {
                StringBuilder sb = new();
                OrbitalReport.ForEach(orbital => sb.Append($"{orbital.OrbitalSymbol}({orbital.PopulationFractionLUMO?.ToString("0.00", CultureInfo.InvariantCulture)})"));
                return sb.ToString();
            }
        }

        [JsonIgnore]
        public List<AtomOrbitalReport> OrbitalReport { get; set; } = [];
    }
}
