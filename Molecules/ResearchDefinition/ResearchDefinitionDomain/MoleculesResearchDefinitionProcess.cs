using CommonDomain;

namespace ResearchDefinitionDomain.Molecule
{
    public sealed record MoleculesResearchDefinitionProcess
    {
        public ProcessType Type { get; init; }

        public MoleculesResearchDefinitionProcessStep[] Steps { get; init; } = [];

    }
}
