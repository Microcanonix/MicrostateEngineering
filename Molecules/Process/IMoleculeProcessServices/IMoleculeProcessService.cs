using CommonDomain;
using MoleculeProcessDomain;

namespace IMoleculeProcessServices
{
    public interface IMoleculeProcessService
    {
        Task<MoleculeGmsResult> HandleImportData(MoleculeContext context);

        Task<MoleculeGmsResult> HandleGeometryOptimization(MoleculeContext context);

        Task<MoleculeGmsResult> HandleElectronicStructure(MoleculeContext context);

        Task<MoleculeGmsResult> HandleFukui(MoleculeContext context);

        Task<MoleculeGmsResult> HandleGeoDiskCharge(MoleculeContext context);

        Task<MoleculeGmsResult> HandleChelpGCharge(MoleculeContext context);

    }
}
