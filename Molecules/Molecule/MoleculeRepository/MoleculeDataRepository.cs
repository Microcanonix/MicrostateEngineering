using IMoleculeRepository;
using IUtilitiesServices;
using MoleculeDomain.MoleculeFile;

namespace MoleculeRepository
{
    public sealed class MoleculeDataRepository : MoleculeFileRepository<MoleculeFileMoleculeData>, IMoleculeDataRepository
    {
        public MoleculeDataRepository(IFileServices fileServices, IDirectoryServices directoryServices)
            :base(fileServices, directoryServices) { }

        public MoleculeFileMoleculeData GetMoleculeDataFile(string directoryPath, string moleculeName)
        {
            return GetMoleculeFile(directoryPath, moleculeName);
        }

        public List<MoleculeFileMoleculeData> GetMoleculeDataFiles(string directoryPath)
        {
            return GetMoleculeFiles(directoryPath);
        }

        public void SaveMoleculeDataFile(string directoryPath, MoleculeFileMoleculeData moleculeFile)
        {
            SaveMoleculeFile(directoryPath, moleculeFile);
        }
    }
}
