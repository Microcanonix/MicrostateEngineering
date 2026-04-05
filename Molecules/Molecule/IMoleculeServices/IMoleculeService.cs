using MoleculeDomain;
using MoleculeDomain.ServiceRequest;

namespace IMoleculeServices
{
    public interface IMoleculeService
    {
        Molecule InitMoleculeFromXyzFile(string xyzFileDirectory, string moleculeName, int charge);

        Molecule UpdateMoleculeFromGmsOutputsGeometryOptimization(GmsCalcCompleteMoleculeRequest request);

        Molecule UpdateMoleculeFromGmsOutputsElectronicStructure(GmsCalcCompleteMoleculeRequest request);

        Molecule UpdateMoleculeFromGmsOutputsFukui(GmsCalcCompleteMoleculeRequest request);

        Molecule UpdateMoleculeFromGmsOutputsChargeGeoDisk(GmsCalcCompleteMoleculeRequest request);

        Molecule UpdateMoleculeFromGmsOutputsChargeChelpG(GmsCalcCompleteMoleculeRequest request);

        void SaveMolecules(List<Molecule> molecules, string moleculesDataDirectory);

        void SaveMoleculesAsXyzFile(List<Molecule> molecules, string xyzFileDirectory);

        Molecule? GetMolecule(string moleculesDataDirectory, string moleculeName);

    }
}
