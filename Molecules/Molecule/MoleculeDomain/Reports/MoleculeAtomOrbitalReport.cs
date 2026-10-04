using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace MoleculeDomain.Reports
{
    public sealed class MoleculeAtomOrbitalReport
    {
        /// <summary>
        /// The name of the molecule this orbital belongs to
        /// </summary>
        public string MoleculeName { get; set; } = string.Empty;

        /// <summary>
        /// The symbol of the atom followed by the position of the atom in the list of atoms
        /// </summary>
        public string AtomID { get; set; } = string.Empty;

        /// <summary>
        /// The mulliken population of this Atom
        /// </summary>
        public double? MullikenPopulation { get; set; }

        /// <summary>
        /// The partitioning of the electron density across the orbitals of this atom ( as fraction of the total atom population )
        /// </summary>
        public string ElectronConfiguration
        {
            get
            {
                StringBuilder sb = new();
                OrbitalReport.ForEach(orbital => sb.Append($"{orbital.OrbitalSymbol}({orbital.PopulationFraction?.ToString("0.00",CultureInfo.InvariantCulture)})"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// The partitioning of the electron HOMO density across the orbitals of this atom ( as fraction of the homo atom population )
        /// </summary>
        public string ElectronConfigurationHOMO
        {
            get
            {
                StringBuilder sb = new();
                OrbitalReport.ForEach(orbital => sb.Append($"{orbital.OrbitalSymbol}({orbital.PopulationFractionHOMO?.ToString("0.00", CultureInfo.InvariantCulture)})"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// The partitioning of the electron LUMO density across the orbitals of this atom ( as fraction of the lumo atom population )
        /// </summary>
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
