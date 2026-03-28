using MoleculeDomain.MoleculeFile;
using MoleculeDomain.ServiceRequest;
using MoleculeDomain.Utilities;

namespace IMoleculeServices
{
    public interface IGmsInputService
    {
        MoleculeFileGmsInput? CreateElectronicStructureGmsInput(GmsCalcInputServiceRequest request);
    }
}
