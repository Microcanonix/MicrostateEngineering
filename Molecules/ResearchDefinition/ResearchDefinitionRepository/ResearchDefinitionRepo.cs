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

        private readonly ResearchDefinitionSettings _settings;


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

        public void DeleteMoleculesResearchDefintion(string researchDefinitionName)
        {
            if (_directoryServices.DirectoryExists(_settings.MoleculesLocation))
            {
                var yamlFiles = _directoryServices.GetFilePaths(_settings.MoleculesLocation, $"{researchDefinitionName}.yaml");
                foreach(var file in yamlFiles)
                {
                    _fileServices.DeleteFile(file);
                }
            }
            else
            {
                _logger.LogError("{MoleculesLocation} does not exist !", _settings.MoleculesLocation);
            }
        }

        public MoleculesResearchDefinition? GetMoleculesResearchDefinition(string researchDefinitionName)
        {
            if (_directoryServices.DirectoryExists(_settings.MoleculesLocation))
            {
                var yamlFiles = _directoryServices.GetFilePaths(_settings.MoleculesLocation, $"{researchDefinitionName}.yaml");
                if ( yamlFiles.Any())
                {
                    var fileContent = _fileServices.ReadFile(yamlFiles.First());
                    return _yamlParser.Parse(fileContent);
                }
                return null;
            }
            else
            {
                _logger.LogError("{MoleculesLocation} does not exist !", _settings.MoleculesLocation);
                return null;
            }
        }

        public List<MoleculesResearchDefinition> GetMoleculesResearchDefinitions()
        {
            List<MoleculesResearchDefinition> result = [];
            if (_directoryServices.DirectoryExists(_settings.MoleculesLocation))
            {
                var yamlFiles = _directoryServices.GetFilePaths(_settings.MoleculesLocation, "*.yaml");
                foreach (var yamlFile in yamlFiles)
                {
                    var fileContent = _fileServices.ReadFile(yamlFile);
                    var researchDefintion = _yamlParser.Parse(fileContent);
                    result.Add(researchDefintion);
                }
            }
            else
            {
                _logger.LogError("{MoleculesLocation} does not exist !", _settings.MoleculesLocation);
            }
            return result;
        }

        public void SaveMoleculesResearchDefintion(MoleculesResearchDefinition researchDefintion)
        {
            if (_directoryServices.DirectoryExists(_settings.MoleculesLocation))
            {
                string content = _yamlParser.Serialize(researchDefintion);
                _fileServices.WriteFile(Path.Combine(_settings.MoleculesLocation, $"{researchDefintion.Name}.yaml"), content);
            }
            else
            {
                _logger.LogError("{MoleculesLocation} does not exist !", _settings.MoleculesLocation);
            }
        }
    }
}
