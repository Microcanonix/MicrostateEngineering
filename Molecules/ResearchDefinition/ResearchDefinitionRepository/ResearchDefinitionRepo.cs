using IResearchDefinitionRepository;
using IUtilitiesServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ResearchDefinitionDomain.GamessCalculation;

namespace ResearchDefinitionRepository
{
    public sealed class ResearchDefinitionRepo : IResearchDefinitionRepo
    {

        private readonly ILogger<ResearchDefinitionRepo>            _logger;

        private readonly IDirectoryServices                         _directoryServices;

        private readonly IFileServices                              _fileServices;

        private readonly IYamlParser<MoleculesResearchDefinition>   _yamlParser;


        public ResearchDefinitionRepo(IDirectoryServices directoryServices,
                                            IFileServices fileServices,
                                            IYamlParser<MoleculesResearchDefinition> yamlParser,
                                                IOptions<ResearchDefinitionSettings> settings,
                                                ILogger<ResearchDefinitionRepo> logger)
        {
            _logger = logger;
            _fileServices = fileServices;
            _directoryServices = directoryServices;
            _yamlParser = yamlParser;
        }


        public List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions(string sourcePath)
        {
            _logger.LogInformation("Reading MoleculesResearchDefinitions from {MoleculesLocation}", sourcePath);
            List<MoleculesResearchDefinition> result = [];
            if (_directoryServices.DirectoryExists(sourcePath))
            {
                var yamlFiles = _directoryServices.GetFilePaths(sourcePath, "*.yaml");
                foreach (var yamlFile in yamlFiles)
                {
                    var fileContent = _fileServices.ReadFile(yamlFile);
                    var researchDefintion = _yamlParser.Parse(fileContent);
                    result.Add(researchDefintion);
                }
            }
            else
            {
                _logger.LogError("{MoleculesLocation} does not exist !", sourcePath);
            }
            return result;
        }
    }
}
