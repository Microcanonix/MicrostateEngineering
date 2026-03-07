using System.Text.Json.Serialization;

namespace MoleculeDomain
{
    public class ElectronPopulation
    {
        public double? Population { get; set; }

        public double? PopulationMinus1 { get; set; }

        public double? PopulationPlus1 { get; set; }

        [JsonIgnore]
        public double? PopulationLUMO => PopulationPlus1 - Population;

        [JsonIgnore]
        public double? PopulationHOMO => Population - PopulationMinus1;
    }
}
