using IMoleculeProcessServices;

namespace MoleculeProcessService
{
    public sealed class MoleculeGmsWorkflowService : IMoleculeWorkflowService
    {
        public async Task RunAsync()
        {
            Console.WriteLine("Workflow Run");
            await Task.CompletedTask;
        }
    }
}
