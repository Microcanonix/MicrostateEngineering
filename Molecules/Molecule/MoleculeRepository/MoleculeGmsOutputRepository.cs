using IMoleculeRepository;
using IUtilitiesServices;
using MoleculeDomain.MoleculeFile;

namespace MoleculeRepository
{
    public class MoleculeGmsOutputRepository : MoleculeFileRepository<MoleculeFileGmsOutput>, IMoleculeGmsOutputRepository
    {
        public MoleculeGmsOutputRepository(IFileServices fileServices, IDirectoryServices directoryServices)
            :base(fileServices, directoryServices)
        {

        }

        public MoleculeFileGmsOutput GetMoleculeGmsOutputFile(string directoryPath, string moleculeName)
        {
            return GetMoleculeFile(directoryPath, moleculeName);
        }

        public List<MoleculeFileGmsOutput> GetMoleculeGmsOutputFiles(string directoryPath)
        {
            return GetMoleculeFiles(directoryPath);
        }

        public void SaveMoleculeGmsOutputFile(string directoryPath, MoleculeFileGmsOutput moleculeFile)
        {
            SaveMoleculeFile(directoryPath, moleculeFile);
        }
    }
}
