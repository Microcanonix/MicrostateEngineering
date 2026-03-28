using MoleculeDomain.FactoryRequest;
using MoleculeDomain.MoleculeFile;

namespace IMoleculeFactory
{
    public interface IGmsCalcInputFactory
    {
        MoleculeFileGmsInput BuildGeoOptGmsInput(GmsCalcInputFactoryRequest request);

        MoleculeFileGmsInput BuildFukuiNeutralInput(GmsCalcInputFactoryRequest request);

        MoleculeFileGmsInput BuildFukuiLUMOInput(GmsCalcInputFactoryRequest request);

        MoleculeFileGmsInput BuildFukuiHOMOInput(GmsCalcInputFactoryRequest request);

        MoleculeFileGmsInput BuildGeoDiskChargeInput(GmsCalcInputFactoryRequest request);

        MoleculeFileGmsInput BuildCHelpGChargeInput(GmsCalcInputFactoryRequest request);

        MoleculeFileGmsInput BuildElectronicStructureInput(GmsCalcInputFactoryRequest request);
    }
}
