namespace ResearchDefinitionDomain.Molecule
{
    public sealed record MoleculesResearchDefinitionProcessStep
    {
        public int Id { get; set; }

        public MoleculesResearchDefinitionProcessStepType Type { get; set; }

        public bool CanExecute { get; set; }

    }
}
