using CommonDomain;
using IMoleculeProcessServices;
using IMoleculeServices;
using Microsoft.Extensions.Logging;
using MoleculeDomain;
using MoleculeDomain.ServiceRequest;
using MoleculeDomain.Utilities;
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
            if (!context.CanExecute)
            {
                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"Molecule {context.MoleculeName} was skipped"
                };
            }
            try
            {
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.GmsInputFolder);
                
                var result = _gmsInputService.CreateGeoOptGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });
                
                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $" {nameof(HandleGeometryOptimization)} succeeded for {context.MoleculeName}"
                };
            }
            catch(Exception e)
            {
                _logger.LogCritical(e, $"Error while {nameof(HandleGeometryOptimization)} for molecule {context.MoleculeName}");
                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<MoleculeGmsResult> HandleElectronicStructure(MoleculeContext context)
        {
            if (!context.CanExecute)
            {
                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"Molecule {context.MoleculeName} was skipped"
                };
            }

            try
            {
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.GmsInputFolder);
                var result = _gmsInputService.CreateElectronicStructureGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"{nameof(HandleElectronicStructure)} was successfully executed for molecule {context.MoleculeName}"
                };
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Error while {nameof(HandleElectronicStructure)} for molecule {context.MoleculeName}");
                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<MoleculeGmsResult> HandleFukui(MoleculeContext context)
        {
            if (!context.CanExecute)
            {
                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"Molecule {context.MoleculeName} was skipped"
                };
            }

            try
            {
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.GmsInputFolder);
                var result = _gmsInputService.CreateFukuiGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"{nameof(HandleFukui)} was successfully executed for molecule {context.MoleculeName}"
                };

            }
            catch(Exception e)
            {
                _logger.LogCritical(e, $"Failed to executie {nameof(HandleFukui)} for molecule {context.MoleculeName}");
                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<MoleculeGmsResult> HandleChelpGCharge(MoleculeContext context)
        {
            if (!context.CanExecute)
            {
                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"Molecule {context.MoleculeName} was skipped"
                };
            }

            try
            {
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.GmsInputFolder);
                var result = _gmsInputService.CreateChelpGChargeGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"{nameof(HandleChelpGCharge)} was successfully executed for molecule {context.MoleculeName}"
                };

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to executie {nameof(HandleChelpGCharge)} for molecule {context.MoleculeName}");
                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }

        public async Task<MoleculeGmsResult> HandleGeoDiskCharge(MoleculeContext context)
        {
            if (!context.CanExecute)
            {
                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"Molecule {context.MoleculeName} was skipped"
                };
            }

            try
            {
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.GmsInputFolder);
                var result = _gmsInputService.CreateGeoDiskChargeGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                return new MoleculeGmsResult()
                {
                    IsSuccess = true,
                    Message = $"{nameof(HandleGeoDiskCharge)} was successfully executed for molecule {context.MoleculeName}"
                };

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"Failed to executie {nameof(HandleGeoDiskCharge)} for molecule {context.MoleculeName}");
                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
    }
}
