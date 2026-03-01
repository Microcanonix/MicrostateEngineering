namespace ResearchDefinitionDomain.Molecule
{
    public sealed record MoleculesResearchDefinitionMolecule
    {
        public required string Name { get; set; }

        public required int Charge { get; set; }
    }
}
