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

        public MoleculeGmsResult HandleImportData(MoleculeContext context)
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

                var xyzDirectory = Path.Combine(context.PackageRoot, context.ResearchName, context.XyzfilesFolder);
                var moleculeDirectory = Path.Combine(context.PackageRoot, context.ResearchName, context.MoleculeDataFolder);
                var existingMolecule = _moleculeService.GetMolecule(moleculeDirectory, context.MoleculeName);
                if (existingMolecule is null )
                {
                    var moleculeFromXyzFile = _moleculeService.InitMoleculeFromXyzFile(xyzDirectory, context.MoleculeName, context.Charge);
                    _moleculeService.SaveMolecules(new List<Molecule>() { moleculeFromXyzFile }, moleculeDirectory); 
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

        public MoleculeGmsResult HandleGeometryOptimization(MoleculeContext context)
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
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.ResearchName, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsInputFolder);
                var gmsOutputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsOutputFolder);
                
                _ = _gmsInputService.CreateGeoOptGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                _ = _moleculeService.UpdateMoleculeFromGmsOutputsGeometryOptimization(new GmsCalcCompleteMoleculeRequest()
                {
                    MoleculeDataFileDirectory = moleculesDataPath,
                    GmsOutputFileDirectory = gmsOutputPath,
                    MoleculeName = context.MoleculeName,
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

        public MoleculeGmsResult HandleElectronicStructure(MoleculeContext context)
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
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.ResearchName, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsInputFolder);
                var gmsOutputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsOutputFolder);

                _ = _gmsInputService.CreateElectronicStructureGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                _ = _moleculeService.UpdateMoleculeFromGmsOutputsElectronicStructure(new GmsCalcCompleteMoleculeRequest()
                {
                    MoleculeDataFileDirectory = moleculesDataPath,
                    GmsOutputFileDirectory = gmsOutputPath,
                    MoleculeName = context.MoleculeName,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
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

        public MoleculeGmsResult HandleFukui(MoleculeContext context)
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
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.ResearchName, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsInputFolder);
                var gmsOutputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsOutputFolder);
                _ = _gmsInputService.CreateFukuiGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                _ = _moleculeService.UpdateMoleculeFromGmsOutputsFukui(new GmsCalcCompleteMoleculeRequest()
                {
                    MoleculeDataFileDirectory = moleculesDataPath,
                    GmsOutputFileDirectory = gmsOutputPath,
                    MoleculeName = context.MoleculeName,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
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

        public MoleculeGmsResult HandleChelpGCharge(MoleculeContext context)
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
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.ResearchName, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsInputFolder);
                var gmsOutputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsOutputFolder);
                _ = _gmsInputService.CreateChelpGChargeGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                _ = _moleculeService.UpdateMoleculeFromGmsOutputsChargeChelpG(new GmsCalcCompleteMoleculeRequest()
                {
                    MoleculeDataFileDirectory = moleculesDataPath,
                    GmsOutputFileDirectory = gmsOutputPath,
                    MoleculeName = context.MoleculeName,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
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

        public MoleculeGmsResult HandleGeoDiskCharge(MoleculeContext context)
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
                var moleculesDataPath = Path.Combine(context.PackageRoot, context.ResearchName, context.MoleculeDataFolder);
                var gmsInputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsInputFolder);
                var gmsOutputPath = Path.Combine(context.PackageRoot, context.ResearchName, context.GmsOutputFolder);
                _ = _gmsInputService.CreateGeoDiskChargeGmsInput(new GmsCalcInputServiceRequest()
                {
                    GmsInputFileDirectory = gmsInputPath,
                    MoleculeFileDirectory = moleculesDataPath,
                    MoleculeName = context.MoleculeName,
                    Charge = context.Charge,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                _ = _moleculeService.UpdateMoleculeFromGmsOutputsChargeGeoDisk(new GmsCalcCompleteMoleculeRequest()
                {
                    MoleculeDataFileDirectory = moleculesDataPath,
                    GmsOutputFileDirectory = gmsOutputPath,
                    MoleculeName = context.MoleculeName,
                    BasisSet = CalcBasisSetTable.GetCalcBasisSetEnum(context.Basisset)
                });

                return new MoleculeGmsResult()
                {
                    IsSuccess = false,
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
