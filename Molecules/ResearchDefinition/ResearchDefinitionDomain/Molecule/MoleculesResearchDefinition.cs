namespace ResearchDefinitionDomain.Molecule
{
    public sealed class MoleculesResearchDefinition
    {
        public required string Name { get; set; }
        public required string PackageRoot { get; set; }
        public required string Xyzfiles { get; set; }
        public required string GmsInput { get; set; }
        public required string GmsOutput { get; set; }
        public required string WorkflowStatusFolder { get; set; }
        public required string MoleculeData { get; set; }
        public required string Basisset { get; set; }
        public MoleculesResearchDefinitionMolecule[] Molecules { get; set; } = [];
        public MoleculesResearchDefinitionProcess[] Processes { get; set; } = [];
    }
}
