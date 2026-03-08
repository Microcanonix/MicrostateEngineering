namespace ResearchDefinitionDomain.Molecule
{
    public sealed record MoleculesResearchDefinitionProcessStep
    {
        public int Id { get; init; }

        public MoleculesResearchDefinitionProcessStepType Type { get; init; }

        public bool CanExecute { get; init; }

    }
}
