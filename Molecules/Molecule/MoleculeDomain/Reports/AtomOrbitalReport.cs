namespace MoleculeDomain.Reports
{
    public class AtomOrbitalReport
    {

        private double? _populationFraction;

        private double? _population;
        private double? _populationHOMO;
        private double? _populationLUMO;

        /// <summary>
        /// The name of the molecule to whom this orbital belongs
        /// </summary>
        public string MoleculeName { get; set; } = string.Empty;

        /// <summary>
        /// The symbol of the atom follow by the position in the list of atoms
        /// </summary>
        public string AtomID { get; set; } = string.Empty;
        public int OrbitalPosition { get; set; } = 0;
        public string OrbitalSymbol { get; set; } = string.Empty;

        /// <summary>
        /// The orbital population devided by the total atom population
        /// </summary>
        public double? PopulationFraction
        {
            get
            {
                return _populationFraction;
            }
            set
            {
                _populationFraction = value.HasValue ? Math.Round(value.Value, 6) : null;
            }
        }

        /// <summary>
        /// The orbital population
        /// </summary>
        public double? Population
        {
            get
            {
                return _population;
            }
            set
            {
                _population = value.HasValue ? Math.Round(value.Value, 6) : null;
            }
        }

        /// <summary>
        /// The homo population of this orbital
        /// </summary>
        public double? PopulationHOMO
        {
            get
            {
                return _populationHOMO;
            }
            set
            {
                _populationHOMO = value.HasValue ? Math.Round(value.Value, 6) : null;
            }
        }

        /// <summary>
        /// The lumo population of this orbital
        /// </summary>
        public double? PopulationLUMO
        {
            get
            {
                return _populationLUMO;
            }
            set
            {
                _populationLUMO = value.HasValue ? Math.Round(value.Value, 6) : null;
            }
        }
    }
}
