namespace ResearchDefinitionDomain.Report
{
    public sealed class MoleculeResearchDefinitionReport
    {
        public required string Name { get; set; }

        public List<MoleculeResearchDefinitionReportItem> MoleculeResult { get; set; } = new();

    }
}
