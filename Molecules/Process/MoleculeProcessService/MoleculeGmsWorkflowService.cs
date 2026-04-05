using CommonDomain;
using Engine.Workflow;
using Engine.WorkflowExecution;
using IMoleculeProcessFactory;
using IMoleculeProcessServices;
using IResearchDefintionService;
using Microsoft.Extensions.Logging;

namespace MoleculeProcessService
{
    public sealed class MoleculeGmsWorkflowService : IMoleculeWorkflowService
    {

        private readonly ILogger<MoleculeGmsWorkflowService> _logger;

        private readonly IResearchDefinitionService _researchDefinitionService;

        private readonly IMoleculeWorkFlowFactory _moleculeWorkFlowFactory;

        private readonly MoleculeGmsWorkflowExecutor _workflowExecutor;


        public MoleculeGmsWorkflowService(ILogger<MoleculeGmsWorkflowService> logger,
                                            IResearchDefinitionService researchDefinitionService,
                                            IMoleculeWorkFlowFactory moleculeWorkFlowFactory,
                                            MoleculeGmsWorkflowExecutor workflowExecutor)
        {

            _logger = logger;
            _researchDefinitionService = researchDefinitionService;
            _moleculeWorkFlowFactory = moleculeWorkFlowFactory;
            _workflowExecutor = workflowExecutor;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Start Running workflow");
            var researchDefinitions = _researchDefinitionService.GetMoleculesResearchDefinitions();
            foreach(var researchDefintion in researchDefinitions)
            {
                var workflows = _moleculeWorkFlowFactory.BuildGmsWorkflow(researchDefintion);
                foreach(var workflow in workflows)
                {
                    var workflowReport = await _workflowExecutor.RunAsync(workflow, new WorkflowExecutorOptions()
                    {
                        MaxDegreeOfParallelism = 1,
                        FailFast = true,
                        SkipDependentsOnFailure = true
                    });
                }
            }
        }

        private void Executor_NodeStateChanged(object? sender, WorkflowNodeStateChangedEventArgs<StepType> e)
        {
            if ( e.NodeId == StepType.import_data && e.State == NodeState.WaitingForInput)
            {

            }
        }
    }
}
