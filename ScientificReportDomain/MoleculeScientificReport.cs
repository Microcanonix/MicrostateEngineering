using MoleculeDomain.Reports;

namespace ScientificReportDomain
{
    public sealed class MoleculeScientificReport
    {
        public required string MoleculeName { get; set; }

        public MoleculeReport? Report { get; set; }

    }
}
