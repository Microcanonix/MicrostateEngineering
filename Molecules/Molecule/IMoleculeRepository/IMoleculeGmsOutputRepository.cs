using MoleculeDomain.MoleculeFile;

namespace IMoleculeRepository
{
    public interface IMoleculeGmsOutputRepository
    {
        MoleculeFileGmsOutput GetMoleculeGmsOutputFile(string directoryPath, string moleculeName);

        List<MoleculeFileGmsOutput> GetMoleculeGmsOutputFiles(string directoryPath);

        void SaveMoleculeGmsOutputFile(string directoryPath, MoleculeFileGmsOutput moleculeFile);

    }
}
