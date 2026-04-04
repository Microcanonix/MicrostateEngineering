using MoleculeDomain.MoleculeFile;

namespace IMoleculeRepository
{
    public interface IMoleculeGmsInputRepository
    {
        MoleculeFileGmsInput GetMoleculeGmsInputFile(string directoryPath, MoleculeFileName moleculeName);

        List<MoleculeFileGmsInput> GetMoleculeGmsInputFiles(string directoryPath);

        void SaveMoleculeGmsInputFile(string directoryPath, MoleculeFileGmsInput moleculeFile);
    }
}
