using IMoleculeFactory;
using IMoleculeRepository;
using IMoleculeServices;
using Microsoft.Extensions.Logging;
using MoleculeDomain;
using MoleculeDomain.MoleculeFile;
using MoleculeDomain.Utilities;
using MoleculeFactory.Conversion;

namespace MoleculeServices
{
    public sealed class MoleculeService : IMoleculeService
    {
        private readonly ILogger<MoleculeService> _logger;

        private readonly IBuildMoleculeFactory _buildMoleculeFactory;

        private readonly IMoleculeXyzRepository _moleculeXyzRepository;

        private readonly IMoleculeDataRepository _moleculeDataRepository;

        public MoleculeService(ILogger<MoleculeService> logger,
                                IBuildMoleculeFactory buildMoleculeFactory,
                                    IMoleculeXyzRepository moleculeXyzRepository,
                                    IMoleculeDataRepository moleculeDataRepository )
 
        {
            _logger = logger;
            _buildMoleculeFactory = buildMoleculeFactory;
            _moleculeXyzRepository = moleculeXyzRepository;
            _moleculeDataRepository = moleculeDataRepository;
        }

        public async Task<Molecule> InitMoleculeFromXyzFileAsync(string xyzFileDirectory, string moleculeName, int charge)
        {
            _logger.LogInformation($"{nameof(InitMoleculeFromXyzFileAsync)} {xyzFileDirectory} {moleculeName} {charge}");
            var moleculeXyzFile = _moleculeXyzRepository.GetMoleculeXyzFile(xyzFileDirectory, moleculeName);
            var result =  _buildMoleculeFactory.BuildMolecule(moleculeXyzFile, moleculeName, charge);
            return await Task.FromResult(result);
        }

        public async Task SaveMoleculesAsXyzFileAsync(List<Molecule> molecules, string xyzFileDirectory)
        {
            foreach(var molecule in molecules)
            {
                _moleculeXyzRepository.SaveMoleculeXyzFile(xyzFileDirectory, _buildMoleculeFactory.BuildMoleculeXyzFile(molecule));
            }
            await Task.CompletedTask;
        }

        public async Task SaveMoleculesAsync(List<Molecule> molecules, string moleculesDataDirectory)
        {
            foreach(Molecule molecule in molecules)
            {
                _moleculeDataRepository.SaveMoleculeDataFile(moleculesDataDirectory, _buildMoleculeFactory.BuildMoleculeDataFile(molecule));
            }
            await Task.CompletedTask;
        }
    }
}
