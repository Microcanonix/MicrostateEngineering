using MoleculeDomain.MoleculeFile;
using MoleculeDomain.ServiceRequest;

namespace IMoleculeServices
{
    public interface IGmsInputService
    {
        MoleculeFileGmsInput? CreateElectronicStructureGmsInput(GmsCalcInputServiceRequest request);

        ( MoleculeFileGmsInput? Neutral, MoleculeFileGmsInput? Homo, MoleculeFileGmsInput? Lumo ) CreateFukuiGmsInput(GmsCalcInputServiceRequest request);

        MoleculeFileGmsInput? CreateGeoOptGmsInput(GmsCalcInputServiceRequest request);

        MoleculeFileGmsInput? CreateChelpGChargeGmsInput(GmsCalcInputServiceRequest request);

        MoleculeFileGmsInput? CreateGoDiskChargeGmsInput(GmsCalcInputServiceRequest request);
    }
}
