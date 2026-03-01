namespace ResearchDefinitionDomain.Molecule
{
    public sealed record MoleculesResearchDefinitionProcess
    {
        public MoleculesResearchDefinitionProcessType Type { get; set; }

        public MoleculesResearchDefinitionProcessStep[] Steps { get; set; } = [];

    }
}
