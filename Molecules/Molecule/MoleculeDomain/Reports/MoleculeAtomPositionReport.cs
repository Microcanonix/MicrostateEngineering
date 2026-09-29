namespace MoleculeDomain.Reports

{
    public class MoleculeAtomPositionReport
    {
        public string MoleculeName { get; set; } = "";
        public int AtomPosition { get; set; }
        public string AtomSymbol { get; set; } = "";
        public double? PosX { get; set; }
        public double? PosY { get; set; }
        public double? PosZ { get; set; }
    }
}
