using CommonDomain;
using IMoleculeFactory;
using IMoleculeRepository;
using IMoleculeServices;
using Microsoft.Extensions.Logging;
using MoleculeDomain;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.ServiceRequest;

namespace MoleculeServices
{
    public sealed class MoleculeService : IMoleculeService
    {
        private readonly ILogger<MoleculeService> _logger;

        private readonly IMoleculesFactory _buildMoleculeFactory;

        private readonly IMoleculeXyzRepository _moleculeXyzRepository;

        private readonly IMoleculeDataRepository _moleculeDataRepository;

        private readonly IMoleculeGmsOutputRepository _moleculeGmsOutputRepository;

        public MoleculeService(ILogger<MoleculeService> logger,
                                IMoleculesFactory buildMoleculeFactory,
                                    IMoleculeXyzRepository moleculeXyzRepository,
                                        IMoleculeDataRepository moleculeDataRepository,
                                            IMoleculeGmsOutputRepository moleculeGmsOutputRepository)
 
        {
            _logger = logger;
            _buildMoleculeFactory = buildMoleculeFactory;
            _moleculeXyzRepository = moleculeXyzRepository;
            _moleculeDataRepository = moleculeDataRepository;
            _moleculeGmsOutputRepository = moleculeGmsOutputRepository;
        }

        public Molecule? GetMolecule(string moleculesDataDirectory, string moleculeName)
        {
            var moleculeData = _moleculeDataRepository.GetMoleculeDataFile(moleculesDataDirectory, new MoleculeFileName(moleculeName));
            if (moleculeData is null) return null;
            return _buildMoleculeFactory.BuildMolecule(moleculeData);
        }

        public void SaveMoleculesAsXyzFile(List<Molecule> molecules, string xyzFileDirectory)
        {
            foreach (var molecule in molecules)
            {
                _moleculeXyzRepository.SaveMoleculeXyzFile(xyzFileDirectory, _buildMoleculeFactory.BuildMoleculeXyzFile(molecule));
            }
        }

        public void SaveMolecules(List<Molecule> molecules, string moleculesDataDirectory)
        {
            foreach (Molecule molecule in molecules)
            {
                _moleculeDataRepository.SaveMoleculeDataFile(moleculesDataDirectory, _buildMoleculeFactory.BuildMoleculeDataFile(molecule));
            }
        }

        public Molecule InitMoleculeFromXyzFile(string xyzFileDirectory, string moleculeName, int charge)
        {
            _logger.LogInformation($"{nameof(InitMoleculeFromXyzFile)} {xyzFileDirectory} {moleculeName} {charge}");
            var moleculeXyzFile = _moleculeXyzRepository.GetMoleculeXyzFile(xyzFileDirectory, new MoleculeFileName(moleculeName));
            var result =  _buildMoleculeFactory.BuildMolecule(moleculeXyzFile, moleculeName, charge);
            return result;
        }


        public Molecule UpdateMoleculeFromGmsOutputsChargeChelpG(GmsCalcCompleteMoleculeRequest request)
        {
            throw new NotImplementedException();
        }

        public Molecule UpdateMoleculeFromGmsOutputsChargeGeoDisk(GmsCalcCompleteMoleculeRequest request)
        {
            throw new NotImplementedException();
        }

        public Molecule UpdateMoleculeFromGmsOutputsElectronicStructuren(GmsCalcCompleteMoleculeRequest request)
        {
            throw new NotImplementedException();
        }

        public Molecule UpdateMoleculeFromGmsOutputsFukui(GmsCalcCompleteMoleculeRequest request)
        {
            throw new NotImplementedException();
        }

        public Molecule UpdateMoleculeFromGmsOutputsGeometryOptimization(GmsCalcCompleteMoleculeRequest request)
        {
            var molecule = GetMolecule(request.MoleculeDataFileDirectory, request.MoleculeName);
            if (molecule is null)
            {
                throw new ServiceException($"{nameof(MoleculeService)}", $"Molecule {request.MoleculeName} does not exist");
            }
            
            var outputFile = _moleculeGmsOutputRepository.
                                        GetMoleculeGmsOutputFile(request.GmsOutputFileDirectory, 
                                                                    new MoleculeFileName(request.MoleculeName,
                                                                                            molecule.Charge,
                                                                                            request.BasisSet,
                                                                                            StepType.geometry_optimization));

            if (!_buildMoleculeFactory.TryCompleteMolecule(molecule, outputFile, OutputFileType.geometry_optimization))
            {
                throw new ServiceException($"{nameof(MoleculeService)}", $"Molecule {request.MoleculeName} failed to complete, gs output data is invalid");
            }

            SaveMolecules([molecule], request.MoleculeDataFileDirectory);

            return molecule;
        }
    }
}
