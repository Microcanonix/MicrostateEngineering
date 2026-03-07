using IMoleculeRepository;
using IUtilitiesServices;
using MoleculeDomain.MoleculeFile;

namespace MoleculeRepository
{
    public sealed class MoleculeGmsInputRepository : MoleculeFileRepository<MoleculeFileGmsInput>, IMoleculeGmsInputRepository
    {

        public MoleculeGmsInputRepository(IFileServices fileServices, IDirectoryServices directoryServices)
            : base(fileServices, directoryServices) { }


        public MoleculeFileGmsInput GetMoleculeGmsInputFile(string directoryPath, string moleculeName)
        {
            return GetMoleculeFile(directoryPath, moleculeName);
        }

        public List<MoleculeFileGmsInput> GetMoleculeGmsInputFiles(string directoryPath)
        {
           return GetMoleculeFiles(directoryPath);
        }

        public void SaveMoleculeGmsInputFile(string directoryPath, MoleculeFileGmsInput moleculeFile)
        {
            SaveMoleculeFile(directoryPath, moleculeFile);
        }
    }
}
