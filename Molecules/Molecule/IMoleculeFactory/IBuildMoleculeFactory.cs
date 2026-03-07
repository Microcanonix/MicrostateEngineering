using MoleculeDomain;
using MoleculeDomain.MoleculeFile;

namespace IMoleculeFactory
{
    public interface IBuildMoleculeFactory
    {
        Molecule BuildMolecule(MoleculeFileMoleculeData moleculeData);

        Molecule BuildMolecule(MoleculeFileXyz moleculeFileXyz, string name, int charge);

        Molecule CompleteMolecule(Molecule molecule, MoleculeFileGmsOutput moleculeFileGmsOutput);

        MoleculeFileMoleculeData BuildMoleculeDataFile(Molecule molecule);

        MoleculeFileXyz BuildMoleculeXyzFile(Molecule molecule);

    }
}
