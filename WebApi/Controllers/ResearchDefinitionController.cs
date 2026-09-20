using IResearchDefintionService;
using IUtilitiesServices;
using Microsoft.AspNetCore.Mvc;
using ResearchDefinitionDomain.GamessCalculation;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResearchDefinitionController : ControllerBase
    {
        private readonly IResearchDefinitionService _researchDefinitionService;
        private readonly IYamlParser<MoleculesResearchDefinition> _yamlParser;

        public ResearchDefinitionController(
            IResearchDefinitionService researchDefinitionService,
            IYamlParser<MoleculesResearchDefinition> yamlParser)
        {
            _researchDefinitionService = researchDefinitionService;
            _yamlParser = yamlParser;
        }

        [HttpGet(Name = "ResearchDefinition")]
        public IEnumerable<MoleculesResearchDefinition> Get()
        {
            return _researchDefinitionService.GetMoleculesResearchDefinitions();
        }

        [HttpGet("{researchDefinitionName}/yaml")]
        public IActionResult GetYaml(string researchDefinitionName)
        {
            var researchDefinition = _researchDefinitionService
                .GetMoleculesResearchDefinitions()
                .FirstOrDefault(definition =>
                    string.Equals(definition.Name, researchDefinitionName, StringComparison.OrdinalIgnoreCase));

            if (researchDefinition == null)
            {
                return NotFound();
            }

            return Content(_yamlParser.Serialize(researchDefinition), "text/yaml");
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

        [HttpPost("yaml")]
        public IActionResult SaveYaml([FromBody] SaveResearchDefinitionYamlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Yaml))
            {
                return BadRequest();
            }

            var researchDefinition = _yamlParser.Parse(request.Yaml);
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

    public sealed record SaveResearchDefinitionYamlRequest(string Yaml);
}
