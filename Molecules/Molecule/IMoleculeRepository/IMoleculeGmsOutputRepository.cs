using MoleculeDomain.MoleculeFile;

namespace IMoleculeRepository
{
    public interface IMoleculeGmsOutputRepository
    {
        MoleculeFileGmsOutput GetMoleculeGmsOutputFile(string directoryPath, MoleculeFileName moleculeName);

        List<MoleculeFileGmsOutput> GetMoleculeGmsOutputFiles(string directoryPath);

        void SaveMoleculeGmsOutputFile(string directoryPath, MoleculeFileGmsOutput moleculeFile);

    }
}
