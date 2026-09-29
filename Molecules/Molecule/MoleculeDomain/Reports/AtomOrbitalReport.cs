namespace MoleculeDomain.Reports
{
    public class AtomOrbitalReport
    {

        private double? _populationFraction;
        private double? _populationFractionHOMO;
        private double? _populationFractionLUMO;


        private double? _population;
        private double? _populationHOMO;
        private double? _populationLUMO;


        public string MoleculeName { get; set; } = "";
        public string AtomID { get; set; } = "";
        public int OrbitalPosition { get; set; } = 0;
        public string OrbitalSymbol { get; set; } = "";
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
        public double? PopulationFractionHOMO
        {
            get
            {
                return _populationFractionHOMO;
            }
            set
            {
                _populationFractionHOMO = value.HasValue ? Math.Round(value.Value, 6) : null;
            }
        }
        public double? PopulationFractionLUMO
        {
            get
            {
                return _populationFractionLUMO;
            }
            set
            {
                _populationFractionLUMO = value.HasValue ? Math.Round(value.Value, 6) : null;
            }
        }
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
