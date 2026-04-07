using CommonDomain;
using Engine.Workflow;
using Engine.WorkflowExecution;
using IMoleculeProcessFactory;
using IMoleculeProcessServices;
using IResearchDefintionService;
using Microsoft.Extensions.Logging;
using ResearchDefinitionDomain.GamessCalculation.Report;

namespace MoleculeProcessService
{
    public sealed class MoleculeGmsWorkflowService : IMoleculeWorkflowService
    {

        private readonly ILogger<MoleculeGmsWorkflowService> _logger;

        private readonly IResearchDefinitionService _researchDefinitionService;

        private readonly IResearchDefinitionReportService _researchDefinitionReportService;

        private readonly IMoleculeWorkFlowFactory _moleculeWorkFlowFactory;

        private readonly MoleculeGmsWorkflowExecutor _workflowExecutor;


        public MoleculeGmsWorkflowService(ILogger<MoleculeGmsWorkflowService> logger,
                                            IResearchDefinitionService researchDefinitionService,
                                            IResearchDefinitionReportService researchDefinitionReportService,
                                            IMoleculeWorkFlowFactory moleculeWorkFlowFactory,
                                            MoleculeGmsWorkflowExecutor workflowExecutor)
        {

            _logger = logger;
            _researchDefinitionService = researchDefinitionService;
            _moleculeWorkFlowFactory = moleculeWorkFlowFactory;
            _workflowExecutor = workflowExecutor;
            _researchDefinitionReportService = researchDefinitionReportService;
        }

        public async Task RunAsync()
        {          
            var researchDefinitions = _researchDefinitionService.GetMoleculesResearchDefinitions();
            foreach(var researchDefinition in researchDefinitions)
            {
                _logger.LogInformation($"Start Running workflow {researchDefinition.Name}");
                var currentReport =_researchDefinitionReportService.Read(researchDefinition.Name);
                if ( currentReport is null )
                {
                    currentReport = new MoleculeResearchDefinitionReport()
                    {
                        Name = researchDefinition.Name,
                        MoleculeResult = researchDefinition.Molecules.Select(x => new MoleculeResearchDefinitionReportItem()
                        {
                            MoleculeName = x.Name,
                            Succeeded = false
                        }).ToList()
                    };
                }

                var workflows = _moleculeWorkFlowFactory.BuildGmsWorkflow(researchDefinition);
                foreach (var workflow in workflows)
                {
                    var item = currentReport.MoleculeResult.Find(x => x.MoleculeName == workflow.MoleculeName);
                    var workflowReport = await _workflowExecutor.RunAsync(workflow, new WorkflowExecutorOptions()
                    {
                        MaxDegreeOfParallelism = 1,
                        FailFast = false,
                        SkipDependentsOnFailure = true
                    });

                    if (workflowReport.Succeeded)
                    {
                        item?.Succeeded = true;
                    }
                    else
                    {
                        item?.Succeeded = false;
                    }

                    _researchDefinitionReportService.Save(currentReport);
                }
                _logger.LogInformation($"End Running workflow {researchDefinition.Name}");
            }

            _logger.LogInformation("Finished Running workflow");
        }

        private void Executor_NodeStateChanged(object? sender, WorkflowNodeStateChangedEventArgs<StepType> e)
        {
            if ( e.NodeId == StepType.import_data && e.State == NodeState.WaitingForInput)
            {

            }
        }
    }
}
