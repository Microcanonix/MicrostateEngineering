using IScientificReportServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class ScientificReportController : ControllerBase
    {
        private readonly IScientificReportService _scientificReportService;

        public ScientificReportController(IScientificReportService scientificReportService)
        {
            _scientificReportService = scientificReportService;
        }

        [HttpGet("{name}", Name = "ResearchDefinitionScientificReport")]
        public IActionResult Get([FromRoute]string name)
        {
            var result = _scientificReportService.GetResearchDefinitionReport(name);
            if ( result is null )
            {
                return Ok();
            }
            return Ok(result);
        }

    }
}
