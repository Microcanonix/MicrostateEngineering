using CommonDomain;
using MoleculeProcessDomain;

namespace IMoleculeProcessServices
{
    public interface IMoleculeProcessService
    {
        MoleculeGmsResult HandleImportData(MoleculeContext context);

        MoleculeGmsResult HandleGeometryOptimization(MoleculeContext context);

        MoleculeGmsResult HandleElectronicStructure(MoleculeContext context);

        MoleculeGmsResult HandleFukui(MoleculeContext context);

        MoleculeGmsResult HandleGeoDiskCharge(MoleculeContext context);

        MoleculeGmsResult HandleChelpGCharge(MoleculeContext context);

    }
}
