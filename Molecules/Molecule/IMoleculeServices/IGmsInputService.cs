using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Utilities;

namespace IMoleculeServices
{
    public interface IGmsInputService
    {
        MoleculeFileGmsInput? CreateGmsInput(string gmsInputDirectory, string moleculeDirectory,
                                                            string moleculeName, CalcBasisSetCodeEnum basisSet);
    }
}
