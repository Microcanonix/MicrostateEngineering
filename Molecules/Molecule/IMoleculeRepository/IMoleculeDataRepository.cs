using MoleculeDomain.MoleculeFile;

namespace IMoleculeRepository
{
    public interface IMoleculeDataRepository
    {
        MoleculeFileMoleculeData GetMoleculeDataFile(string directoryPath, string moleculeName);

        List<MoleculeFileMoleculeData> GetMoleculeDataFiles(string directoryPath);

        void SaveMoleculeDataFile(string directoryPath, MoleculeFileMoleculeData moleculeFile);
    }
}
