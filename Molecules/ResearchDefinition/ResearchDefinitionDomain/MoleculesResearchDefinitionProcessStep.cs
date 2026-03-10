using CommonDomain;

namespace ResearchDefinitionDomain.Molecule
{
    public sealed record MoleculesResearchDefinitionProcessStep
    {
        public int Id { get; init; }

        public StepType Type { get; init; }

        public bool CanExecute { get; init; }

    }
}
