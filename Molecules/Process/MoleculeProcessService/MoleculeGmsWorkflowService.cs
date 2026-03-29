using CommonDomain;
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


        public MoleculeGmsWorkflowService(ILogger<MoleculeGmsWorkflowService> logger,
                                            IResearchDefinitionService researchDefinitionService,
                                            IMoleculeWorkFlowFactory moleculeWorkFlowFactory)
        {

            _logger = logger;
            _researchDefinitionService = researchDefinitionService;
            _moleculeWorkFlowFactory = moleculeWorkFlowFactory;
        }



        public async Task RunAsync()
        {
            _logger.LogInformation("Start Running workflow");
            var researchDefinitions = _researchDefinitionService.GetMoleculesResearchDefinitions();
            foreach(var researchDefintion in researchDefinitions)
            {
                var workflows = _moleculeWorkFlowFactory.BuildGmsWorkflow(researchDefintion);
                var executor = new WorkflowExecutor<StepType>();
                foreach(var workflow in workflows)
                {
                    var workflowReport = await executor.RunAsync(workflow, new WorkflowExecutorOptions()
                    {
                        MaxDegreeOfParallelism = 1,
                        FailFast = true,
                        SkipDependentsOnFailure = true

                    });
                }
            }
            await Task.CompletedTask;
        }
    }
}
