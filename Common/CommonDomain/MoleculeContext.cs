namespace CommonDomain
{
    public sealed class MoleculeContext
    {
        public required string Basisset             { get; set; }
        public required string MoleculeName         { get; set; }

        public required string PackageRoot                  { get; set; }
        public required string XyzfilesFolder             { get; set; }
        public required string GmsInputFolder             { get; set; }
        public required string GmsOutputFolder            { get; set; }
        public required string WorkflowStatusFolder { get; set; }
        public required string MoleculeDataFolder   { get; set; }

    }
}
