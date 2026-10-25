using MoleculeDomain.Reports;

namespace ScientificReportDomain
{
    public sealed class ReseachDefinitionScientificReport
    {
        public required string ResearchDefinitionName { get; set; }

        public List<MoleculeScientificReport> MoleculeReports { get; set; } = new();
    
    }
}
