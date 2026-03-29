using CommonDomain;

namespace ResearchDefinitionDomain
{
    public sealed class MoleculesResearchDefinitionProcessDependency
    {
        public required StepType Dependency { get; set; }

        public required StepType Dependant { get; set; }


    }
}
