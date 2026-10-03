

using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Reports;

namespace IMoleculeRepository
{
    public interface IMoleculeReportRepository
    {
        public void Save(string directoryPath, MoleculeFileName fileName, MoleculeReport moleculeReport);
    }
}
