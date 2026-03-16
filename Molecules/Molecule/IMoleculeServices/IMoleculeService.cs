using MoleculeDomain;

namespace IMoleculeServices
{
    public interface IMoleculeService
    {
        Task<Molecule> InitMoleculeFromXyzFileAsync(string xyzFileDirectory, string moleculeName, int charge);
        
        Task SaveMoleculesAsync(List<Molecule> molecules, string moleculesDataDirectory);

        Task SaveMoleculesAsXyzFileAsync(List<Molecule> molecules, string xyzFileDirectory);

    }
}
