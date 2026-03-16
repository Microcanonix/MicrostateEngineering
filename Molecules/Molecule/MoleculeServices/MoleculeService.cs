using IMoleculeFactory;
using IMoleculeRepository;
using IMoleculeServices;
using Microsoft.Extensions.Logging;
using MoleculeDomain;

namespace MoleculeServices
{
    public sealed class MoleculeService : IMoleculeService
    {
        private readonly ILogger<MoleculeService> _logger;

        private readonly IBuildMoleculeFactory _buildMoleculeFactory;

        private readonly IMoleculeXyzRepository _moleculeXyzRepository;

        public MoleculeService(ILogger<MoleculeService> logger,
                                IBuildMoleculeFactory buildMoleculeFactory,
                                    IMoleculeXyzRepository moleculeXyzRepository)
        {
            _logger = logger;
            _buildMoleculeFactory = buildMoleculeFactory;
            _moleculeXyzRepository = moleculeXyzRepository;
        }

        public async Task<Molecule> InitMoleculeFromXyzFileAsync(string xyzFileDirectory, string moleculeName, int charge)
        {
            _logger.LogInformation($"{nameof(InitMoleculeFromXyzFileAsync)} {xyzFileDirectory} {moleculeName} {charge}");
            var moleculeXyzFile = _moleculeXyzRepository.GetMoleculeXyzFile(xyzFileDirectory, moleculeName);
            var result =  _buildMoleculeFactory.BuildMolecule(moleculeXyzFile, moleculeName, charge);
            return await Task.FromResult(result);
        }

        public Task SaveMoleculesAsXyzFileAsync(List<Molecule> molecules, string xyzFileDirectory)
        {
            throw new NotImplementedException();
        }

        public Task SaveMoleculesAsync(List<Molecule> molecules, string moleculesDataDirectory)
        {
            throw new NotImplementedException();
        }
    }
}
