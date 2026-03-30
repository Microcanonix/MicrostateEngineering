using IMoleculeFactory;
using IMoleculeRepository;
using IMoleculeServices;
using MoleculeDomain.FactoryRequest;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.ServiceRequest;

namespace MoleculeServices
{
    public sealed class GmsInputService : IGmsInputService
    {

        private readonly IMoleculeService _moleculeService;

        private readonly IGmsCalcInputFactory _gmsCalcInputFactory;

        private readonly IMoleculeGmsInputRepository _moleculeGmsInputRepository;


        public GmsInputService(IMoleculeService moleculeService,
                               IGmsCalcInputFactory gmsCalcInputFactory,
                                IMoleculeGmsInputRepository moleculeGmsInputRepository)
        {
            _moleculeService = moleculeService;
            _gmsCalcInputFactory = gmsCalcInputFactory;
            _moleculeGmsInputRepository = moleculeGmsInputRepository;
        }

        public MoleculeFileGmsInput? CreateChelpGChargeGmsInput(GmsCalcInputServiceRequest request)
        {
            var molecule = _moleculeService.GetMolecule(request.MoleculeFileDirectory, request.MoleculeName);
            if (molecule is null) return null;

            var result = _gmsCalcInputFactory.BuildCHelpGChargeInput(new GmsCalcInputFactoryRequest()
            {
                MoleculeName = request.MoleculeName,
                Charge = request.Charge,
                BasisSet = request.BasisSet,
                Atoms = molecule.Atoms.OrderBy(x => x.PositionInMolecule).ToList()
            });

            _moleculeGmsInputRepository.SaveMoleculeGmsInputFile(request.GmsInputFileDirectory, result);

            return result;
        }

        public MoleculeFileGmsInput? CreateElectronicStructureGmsInput(GmsCalcInputServiceRequest request)
        {
            var molecule = _moleculeService.GetMolecule(request.MoleculeFileDirectory, request.MoleculeName);
            if (molecule is null) return null;
            
            var result = _gmsCalcInputFactory.BuildElectronicStructureInput(new GmsCalcInputFactoryRequest()
            {
                MoleculeName = request.MoleculeName,
                Charge = request.Charge,
                BasisSet = request.BasisSet,
                Atoms = molecule.Atoms.OrderBy(x => x.PositionInMolecule).ToList()
            });

            _moleculeGmsInputRepository.SaveMoleculeGmsInputFile(request.GmsInputFileDirectory, result);

            return result;
        }

        public (MoleculeFileGmsInput? Neutral, MoleculeFileGmsInput? Homo, MoleculeFileGmsInput? Lumo) CreateFukuiGmsInput(GmsCalcInputServiceRequest request)
        {
            var molecule = _moleculeService.GetMolecule(request.MoleculeFileDirectory, request.MoleculeName);
            if (molecule is null) return (null,null,null);

            var resultNeutral = _gmsCalcInputFactory.BuildFukuiNeutralInput(new GmsCalcInputFactoryRequest()
            {
                MoleculeName = request.MoleculeName,
                Charge = request.Charge,
                BasisSet = request.BasisSet,
                Atoms = molecule.Atoms.OrderBy(x => x.PositionInMolecule).ToList()
            });


            var resultHomo = _gmsCalcInputFactory.BuildFukuiHOMOInput(new GmsCalcInputFactoryRequest()
            {
                MoleculeName = request.MoleculeName,
                Charge = request.Charge,
                BasisSet = request.BasisSet,
                Atoms = molecule.Atoms.OrderBy(x => x.PositionInMolecule).ToList()
            });


            var resultLumo = _gmsCalcInputFactory.BuildFukuiLUMOInput(new GmsCalcInputFactoryRequest()
            {
                MoleculeName = request.MoleculeName,
                Charge = request.Charge,
                BasisSet = request.BasisSet,
                Atoms = molecule.Atoms.OrderBy(x => x.PositionInMolecule).ToList()
            });

            _moleculeGmsInputRepository.SaveMoleculeGmsInputFile(request.GmsInputFileDirectory, resultNeutral);
            _moleculeGmsInputRepository.SaveMoleculeGmsInputFile(request.GmsInputFileDirectory, resultHomo);
            _moleculeGmsInputRepository.SaveMoleculeGmsInputFile(request.GmsInputFileDirectory, resultLumo);

            return (resultNeutral, resultHomo, resultLumo);
        }

        public MoleculeFileGmsInput? CreateGeoOptGmsInput(GmsCalcInputServiceRequest request)
        {
            var molecule = _moleculeService.GetMolecule(request.MoleculeFileDirectory, request.MoleculeName);
            if (molecule is null) return null;

            var result = _gmsCalcInputFactory.BuildGeoOptGmsInput(new GmsCalcInputFactoryRequest()
            {
                MoleculeName = request.MoleculeName,
                Charge = request.Charge,
                BasisSet = request.BasisSet,
                Atoms = molecule.Atoms.OrderBy(x => x.PositionInMolecule).ToList()
            });

            _moleculeGmsInputRepository.SaveMoleculeGmsInputFile(request.GmsInputFileDirectory, result);

            return result;
        }

        public MoleculeFileGmsInput? CreateGeoDiskChargeGmsInput(GmsCalcInputServiceRequest request)
        {
            var molecule = _moleculeService.GetMolecule(request.MoleculeFileDirectory, request.MoleculeName);
            if (molecule is null) return null;

            var result = _gmsCalcInputFactory.BuildGeoDiskChargeInput(new GmsCalcInputFactoryRequest()
            {
                MoleculeName = request.MoleculeName,
                Charge = request.Charge,
                BasisSet = request.BasisSet,
                Atoms = molecule.Atoms.OrderBy(x => x.PositionInMolecule).ToList()
            });

            _moleculeGmsInputRepository.SaveMoleculeGmsInputFile(request.GmsInputFileDirectory, result);

            return result;
        }
    }
}
