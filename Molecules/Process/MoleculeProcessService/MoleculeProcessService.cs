using CommonDomain;
using IMoleculeProcessServices;
using IMoleculeServices;
using Microsoft.Extensions.Logging;
using MoleculeDomain;
using MoleculeProcessDomain;

namespace MoleculeProcessService
{
    public sealed class MoleculeProcessService : IMoleculeProcessService
    {
        private readonly ILogger<MoleculeProcessService> _logger;

        private readonly IMoleculeService _moleculeService;

        private readonly IGmsInputService _gmsInputService;

        public MoleculeProcessService(ILogger<MoleculeProcessService> logger,
                                        IMoleculeService moleculeService,
                                            IGmsInputService gmsInputService )
        {
            _logger = logger;
            _moleculeService = moleculeService;
            _gmsInputService = gmsInputService;
        }

        public async Task<MoleculeGmsResult> HandleImportData(MoleculeContext context)
        {
            try
            {
                if (! context.CanExecute )
                {
                    return new MoleculeGmsResult()
                    {
                        IsSuccess = true,
                        Message = $"Molecule {context.MoleculeName} was skipped"
                    };
                }

                MoleculeGmsResult result;
                var xyzDirectory = Path.Combine(context.PackageRoot, context.XyzfilesFolder);
                var moleculeDirectory = Path.Combine(context.PackageRoot, context.MoleculeDataFolder);
                var existingMolecule = _moleculeService.GetMolecule(moleculeDirectory, context.MoleculeName);
                if (existingMolecule is null )
                {
                    var moleculeFromXyzFile = await _moleculeService.InitMoleculeFromXyzFileAsync(xyzDirectory, context.MoleculeName, context.Charge);
                    await _moleculeService.SaveMoleculesAsync(new List<Molecule>() { moleculeFromXyzFile }, moleculeDirectory); 
                    result = new MoleculeGmsResult()
                    {
                        IsSuccess = true,
                        Message = $"Molecule {moleculeFromXyzFile.Name} was initialised"
                    };
                }
                else
                {
                    result = new MoleculeGmsResult()
                    {
                        IsSuccess = true,
                        Message = $"Molecule {existingMolecule.Name} allready existed"
                    };
                }
                return result;
            }
            catch(Exception exc)
            {
                _logger.LogCritical(exc, "An error occured while Importing Data");
                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
                    Message = $"An unexpected error happened {exc.Message}"
                };
            }
        }

        public async Task<MoleculeGmsResult> HandleGeometryOptimization(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleElectronicStructure(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleFukui(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleChelpGCharge(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }

        public async Task<MoleculeGmsResult> HandleGeoDiskCharge(MoleculeContext context)
        {
            return await Task.FromResult(new MoleculeGmsResult()
            {
                IsSuccess = false,
                Message = "NotImplemented"
            });
        }
    }
}
