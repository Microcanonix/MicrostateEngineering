using IResearchDefinitionRepository;
using IUtilitiesServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ResearchDefinitionDomain.Molecule;
using ResearchDefinitionDomain.Settings;

namespace ResearchDefinitionRepository
{
    public sealed class ResearchDefinitionRepo : IResearchDefinitionRepo
    {

        private readonly ILogger<ResearchDefinitionRepo>            _logger;

        private readonly IDirectoryServices                         _directoryServices;

        private readonly IFileServices                              _fileServices;

        private readonly IYamlParser<MoleculesResearchDefinition>   _yamlParser;

        private readonly ResearchDefinitionSettings                 _settings;


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
            _settings = settings.Value;
        }


        public List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions()
        {
            var sourcePath = _settings.MoleculesLocation;
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
