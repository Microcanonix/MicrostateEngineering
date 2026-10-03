using MoleculeDomain;
using MoleculeDomain.Reports;

namespace IMoleculeServices
{
    public interface IMoleculeReportService
    {
        void SaveForMolecule(string reportDataDirectory, Molecule molecule);

        MoleculeReport? Get(string moleculesDataDirectory, string moleculeName);
    }
}
