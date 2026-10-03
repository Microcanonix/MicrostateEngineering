using IMoleculeFactory;
using IMoleculeRepository;
using IMoleculeServices;
using MoleculeDomain;
using MoleculeDomain.Reports;

namespace MoleculeServices
{
    public sealed class MoleculeReportService(IMoleculeReportFactory moleculeReportFactory
                                                , IMoleculeService moleculeService
                                                , IMoleculeReportRepository moleculeReportRepository)
                            : IMoleculeReportService
    {
        public MoleculeReport? Get(string moleculesDataDirectory, string moleculeName)
        {
            var molecule = moleculeService.GetMolecule(moleculesDataDirectory, moleculeName);
            return moleculeReportFactory.GetMoleculeReport(molecule);
        }

        public void SaveForMolecule(string reportDataDirectory, Molecule molecule)
        {
            var report = moleculeReportFactory.GetMoleculeReport(molecule);
            if ( report != null)
            {
                moleculeReportRepository.Save(reportDataDirectory, new MoleculeDomain.MoleculeFile.MoleculeFileName(molecule.Name), report);
            }
        }
    }
}
