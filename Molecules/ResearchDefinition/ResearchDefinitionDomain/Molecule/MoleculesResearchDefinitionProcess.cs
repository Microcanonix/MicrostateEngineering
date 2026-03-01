namespace ResearchDefinitionDomain.Molecule
{
    public sealed record MoleculesResearchDefinitionProcess
    {
        public MoleculesResearchDefinitionProcessType Type { get; init; }

        public MoleculesResearchDefinitionProcessStep[] Steps { get; init; } = [];

    }
}
