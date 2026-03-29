using CommonDomain;
using Engine.Workflow;
using IMoleculeProcessFactory;
using IMoleculeProcessServices;
using MoleculeProcessDomain;
using ResearchDefinitionDomain;

namespace MoleculeProcessFactory
{
    public sealed class MoleculeWorkFlowFactory : IMoleculeWorkFlowFactory
    {

        private readonly IMoleculeProcessService _moleculeProcessService;

        public MoleculeWorkFlowFactory(IMoleculeProcessService moleculeProcessService)
        {
            _moleculeProcessService = moleculeProcessService;
        }

        public List<MoleculeGmsWorkflow> BuildGmsWorkflow(MoleculesResearchDefinition researchDefinition)
        {
            List<MoleculeGmsWorkflow> result = new List<MoleculeGmsWorkflow>();

            foreach (var molecule in researchDefinition.Molecules)
            {
                var currentWorkflow = new MoleculeGmsWorkflow();
                foreach(var process in researchDefinition.Processes)
                {
                    if ( process.Type == ProcessType.moleculeproperties )
                    {
                        MoleculeContext context = new()
                        {
                            Basisset = researchDefinition.Basisset,
                            PackageRoot = researchDefinition.PackageRoot,
                            XyzfilesFolder = researchDefinition.Xyzfiles,
                            GmsInputFolder = researchDefinition.GmsInput,
                            GmsOutputFolder = researchDefinition.GmsOutput,
                            MoleculeDataFolder = researchDefinition.MoleculeData,
                            WorkflowStatusFolder = researchDefinition.WorkflowStatusFolder,
                            MoleculeName = molecule.Name,
                            Charge = molecule.Charge
                        };

                        foreach(var step in process.Steps)
                        {
                            switch (step.Type)
                            {
                                case StepType.dummy:
                                    break;
                                case StepType.import_data:
                                    currentWorkflow.AddNode(new WorkflowNode<StepType>(step.Type, async (ct) =>
                                    {
                                        var result = await _moleculeProcessService.HandleImportData(context with { CanExecute = step.CanExecute });
                                        return result.IsSuccess ? NodeResult.Success(result.Message) : NodeResult.Failure(result.Message);
                                    }));
                                    break;
                                case StepType.geometry_optimization:
                                    currentWorkflow.AddNode(new WorkflowNode<StepType>(step.Type, async (ct) =>
                                    {
                                        var result = await _moleculeProcessService.HandleGeometryOptimization(context with { CanExecute = step.CanExecute });
                                        return result.IsSuccess ? NodeResult.Success(result.Message) : NodeResult.Failure(result.Message);
                                    }));
                                    break;
                                case StepType.electronic_structure:
                                    currentWorkflow.AddNode(new WorkflowNode<StepType>(step.Type, async (ct) =>
                                    {
                                        var result = await _moleculeProcessService.HandleElectronicStructure(context with { CanExecute = step.CanExecute });
                                        return result.IsSuccess ? NodeResult.Success(result.Message) : NodeResult.Failure(result.Message);
                                    }));
                                    break;
                                case StepType.fukui_calculation:
                                    currentWorkflow.AddNode(new WorkflowNode<StepType>(step.Type, async (ct) =>
                                    {
                                        var result = await _moleculeProcessService.HandleFukui(context with { CanExecute = step.CanExecute });
                                        return result.IsSuccess ? NodeResult.Success(result.Message) : NodeResult.Failure(result.Message);
                                    }));
                                    break;
                                case StepType.charge_geodisk:
                                    currentWorkflow.AddNode(new WorkflowNode<StepType>(step.Type, async (ct) =>
                                    {
                                        var result = await _moleculeProcessService.HandleGeoDiskCharge(context with { CanExecute = step.CanExecute });
                                        return result.IsSuccess ? NodeResult.Success(result.Message) : NodeResult.Failure(result.Message); ;
                                    }));
                                    break;
                                case StepType.charge_chelpg:
                                    currentWorkflow.AddNode(new WorkflowNode<StepType>(step.Type, async (ct) =>
                                    {
                                        var result = await _moleculeProcessService.HandleChelpGCharge(context with { CanExecute = step.CanExecute });
                                        return result.IsSuccess ? NodeResult.Success(result.Message) : NodeResult.Failure(result.Message);
                                    }));
                                    break;
                            }
                        }
                        foreach(var edge in process.Dependencies)
                        {

                            currentWorkflow.AddDependency(edge.Dependency, edge.Dependant);
                        }
                    }
                    result.Add(currentWorkflow);
                }
            }
            return result;
        }
    }
}