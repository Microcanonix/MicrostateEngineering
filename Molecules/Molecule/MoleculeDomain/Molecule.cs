using CoreDomain;
using System.Text.Json.Serialization;

namespace MoleculeDomain
{
    public class Molecule(string name, int charge = 0)
    {
        public string Name { get; set; } = name;

        public int Charge { get; set; } = charge;

        public List<MoleculeAtom> Atoms { get; set; } = [];

        public List<MoleculeBond> Bonds { get; set; } = [];

        public List<ElectronicPotential> ChelpGElpot { get; set; } = [];

        public List<ElectronicPotential> GeoDiskElpot { get; set; } = [];

        public double? DftEnergy { get; set; }

        public double? HFEnergy { get; set; }

        public double? HFEnergyHOMO { get; set; }

        public double? HFEnergyLUMO { get; set; }

        [JsonIgnore]
        public double? IonisationEnergy => HFEnergyHOMO - HFEnergy;

        [JsonIgnore]
        public double? ElectronAffinitiy => HFEnergy - HFEnergyLUMO;

        [JsonIgnore]
        public double? ChemicalPotential => 0.5 * (IonisationEnergy + ElectronAffinitiy);

        [JsonIgnore]
        public double? Hardness => 0.5 * (IonisationEnergy - ElectronAffinitiy);

    }
}
