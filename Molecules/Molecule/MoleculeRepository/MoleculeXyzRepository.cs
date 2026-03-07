using IMoleculeRepository;
using IUtilitiesServices;
using MoleculeDomain.MoleculeFile;

namespace MoleculeRepository
{
    public sealed class MoleculeXyzRepository : MoleculeFileRepository<MoleculeFileXyz>, IMoleculeXyzRepository
    {             
        public MoleculeXyzRepository(IFileServices fileServices, IDirectoryServices directoryServices)
            :base(fileServices, directoryServices){ }

        public MoleculeFileXyz GetMoleculeXyzFile(string directoryPath, string moleculeName)
        {
            return GetMoleculeFile(directoryPath, moleculeName);
        }

        public List<MoleculeFileXyz> GetMoleculeXyzFiles(string directoryPath)
        {
            return GetMoleculeFiles(directoryPath);
        }

        public void SaveMoleculeXyzFile(string directoryPath, MoleculeFileXyz moleculeFile)
        {
            SaveMoleculeFile(directoryPath, moleculeFile);
        }
    }
}
