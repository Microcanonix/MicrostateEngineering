using IResearchDefinitionRepository;
using IUtilitiesServices;
using Microsoft.Extensions.Options;
using ResearchDefinitionDomain.Report;
using ResearchDefinitionDomain.Settings;
using System.Xml.Linq;

namespace ResearchDefinitionRepository
{
    public sealed class ResearchDefinitionReportRepo : IResearchDefinitionReportRepo
    {
        private readonly IDirectoryServices _directoryServices;
        private readonly IFileServices _fileServices;
        private readonly IJsonParser<MoleculeResearchDefinitionReport> _jsonParser;
        private readonly ResearchDefinitionSettings _settings;

        public ResearchDefinitionReportRepo(IDirectoryServices directoryServices,
                                             IFileServices fileServices,
                                              IJsonParser<MoleculeResearchDefinitionReport> jsonParser,
                                               IOptions<ResearchDefinitionSettings> settings )
        {
            _directoryServices = directoryServices;
            _fileServices = fileServices;
            _jsonParser = jsonParser;
            _settings = settings.Value;
        }

        public MoleculeResearchDefinitionReport? Read(string name)
        { 
           var files = _directoryServices.GetFilePaths(_settings.MoleculesLocation, $"{name}_report.json");
           if (!files.Any()) return null;
            var fileContent = _fileServices.ReadFile(files.First());
            return _jsonParser.Parse(fileContent);
        }

        public void Save(MoleculeResearchDefinitionReport report)
        {
            var fileContent = _jsonParser.Serialize(report);
            _fileServices.WriteFile(Path.Combine(_settings.MoleculesLocation, $"{report.Name}_report.json"), fileContent);
        }
    }
}
