namespace MoleculeDomain.Reports

{
    public sealed class MoleculeAtomPositionReport
    {
        public string MoleculeName { get; set; } = string.Empty;
        public int AtomPosition { get; set; }
        public string AtomSymbol { get; set; } = string.Empty;
        public double? PosX { get; set; }
        public double? PosY { get; set; }
        public double? PosZ { get; set; }
    }
}
