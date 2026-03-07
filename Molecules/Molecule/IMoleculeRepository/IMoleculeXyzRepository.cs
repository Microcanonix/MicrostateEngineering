using MoleculeDomain.MoleculeFile;

namespace IMoleculeRepository
{
    public interface IMoleculeXyzRepository
    {
        MoleculeFileXyz GetMoleculeXyzFile(string directoryPath, string moleculeName);

        List<MoleculeFileXyz> GetMoleculeXyzFiles(string directoryPath);

        void SaveMoleculeXyzFile(string directoryPath, MoleculeFileXyz moleculeFile);

    }
}
