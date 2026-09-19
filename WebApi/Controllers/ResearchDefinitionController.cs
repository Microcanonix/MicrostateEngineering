using IResearchDefintionService;
using Microsoft.AspNetCore.Mvc;
using ResearchDefinitionDomain.GamessCalculation;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResearchDefinitionController : ControllerBase
    {
        private readonly IResearchDefinitionService _researchDefinitionService;

        public ResearchDefinitionController(IResearchDefinitionService researchDefinitionService)
        {
            _researchDefinitionService = researchDefinitionService;
        }

        [HttpGet(Name = "ResearchDefinition")]
        public IEnumerable<MoleculesResearchDefinition> Get()
        {
            return _researchDefinitionService.GetMoleculesResearchDefinitions();
        }

        [HttpPost]
        public IActionResult Save([FromBody] MoleculesResearchDefinition researchDefinition)
        {
            if (researchDefinition == null)
            {
                return BadRequest();
            }

            _researchDefinitionService.SaveMoleculesResearchDefintion(researchDefinition);
            return Ok();
        }

        [HttpDelete("{researchDefintionName}")]
        public IActionResult Delete(string researchDefintionName)
        {
            if (string.IsNullOrWhiteSpace(researchDefintionName))
            {
                return BadRequest();
            }

            _researchDefinitionService.DeleteMoleculesResearchDefintion(researchDefintionName);
            return NoContent();
        }
    }
}
