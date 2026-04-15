using System.Text.Json.Serialization;

namespace MoleculeDomain
{
    public class ElectronPopulation
    {
        public double? Population { get; set; }

        public double? PopulationMinus1Electron { get; set; }

        public double? PopulationPlus1Electron { get; set; }

        [JsonIgnore]
        public double? PopulationLUMO => PopulationPlus1Electron - Population;

        [JsonIgnore]
        public double? PopulationHOMO => Population - PopulationMinus1Electron;
    }
}
