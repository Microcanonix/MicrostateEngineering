using MoleculeDomain;
using MoleculeDomain.ServiceRequest;

namespace IMoleculeServices
{
    public interface IMoleculeService
    {
        Task<Molecule> InitMoleculeFromXyzFileAsync(string xyzFileDirectory, string moleculeName, int charge);

        Task<Molecule> UpdateMoleculeFromGmsOutputsGeometryOptimizaion(GmsCalcCompleteMoleculeRequest request);

        Task<Molecule> UpdateMoleculeFromGmsOutputsElectronicStructuren(GmsCalcCompleteMoleculeRequest request);

        Task<Molecule> UpdateMoleculeFromGmsOutputsFukui(GmsCalcCompleteMoleculeRequest request);

        Task<Molecule> UpdateMoleculeFromGmsOutputsChargeGeoDisk(GmsCalcCompleteMoleculeRequest request);

        Task<Molecule> UpdateMoleculeFromGmsOutputsChargeChelpG(GmsCalcCompleteMoleculeRequest request);

        Task SaveMoleculesAsync(List<Molecule> molecules, string moleculesDataDirectory);

        Task SaveMoleculesAsXyzFileAsync(List<Molecule> molecules, string xyzFileDirectory);

        Molecule? GetMolecule(string moleculesDataDirectory, string moleculeName);

    }
}
