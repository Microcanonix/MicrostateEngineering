using MoleculeDomain;
using MoleculeDomain.Utilities;

namespace CoreDomain
{
    public class MoleculeAtom(Atom atom, PositionVector pos)
    {
        public int? PositionInMolecule { get; set; }

        public Atom Atom { get; init; } = atom;

        public PositionVector Pos { get; set; } = pos;

        public Charge? Charge { get; set; }

        public ElectronPopulation? MullikenPopulation { get; set; }

        public ElectronPopulation? LowdinPopulation { get; set; }

        public List<MoleculeAtomOrbital> Orbitals { get; set; } = [];

        public List<MoleculeBond> Bonds { get; set; } = [];

    }
}
