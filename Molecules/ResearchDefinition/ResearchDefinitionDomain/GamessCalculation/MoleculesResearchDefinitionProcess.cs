using CommonDomain;

namespace ResearchDefinitionDomain.GamessCalculation
{
    public sealed record MoleculesResearchDefinitionProcess
    {
        public ProcessType Type { get; init; }

        public MoleculesResearchDefinitionProcessStep[] Steps { get; init; } = [];

        public MoleculesResearchDefinitionProcessDependency[] Dependencies { get; init; } = [];

    }
}
