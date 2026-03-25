using MoleculeDomain.FactoryRequest;
using MoleculeDomain.MoleculeFile;

namespace IMoleculeFactory
{
    public interface IGmsCalcInputFactory
    {
        MoleculeFileGmsInput BuildGeoOptGmsInput(GmsCalcInputRequest request);

        MoleculeFileGmsInput BuildNeutralInput(GmsCalcInputRequest request);

        MoleculeFileGmsInput BuildFukuiLUMOInput(GmsCalcInputRequest request);

        MoleculeFileGmsInput BuildFukuiHOMOInput(GmsCalcInputRequest request);

        MoleculeFileGmsInput BuildGeoDiskChargeInput(GmsCalcInputRequest request);

        MoleculeFileGmsInput BuildCHelpGChargeInput(GmsCalcInputRequest request);

    }
}
