using IMoleculeProcessServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class WorkflowController : ControllerBase
    {
        private readonly IMoleculeWorkflowService _workflowService;

        public WorkflowController(IMoleculeWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost]
        public async Task<IActionResult> Run([FromQuery, Required] string researchDefintionName)
        {
            if (string.IsNullOrWhiteSpace(researchDefintionName))
            {
                return BadRequest("researchDefintionName is required.");
            }
            var report = await _workflowService.RunAsync(researchDefintionName);
            if (report is null)
            {
                return NotFound($"Research definition '{researchDefintionName}' not found or produced no report.");
            }
            return Ok(report);
        }

    }
}
