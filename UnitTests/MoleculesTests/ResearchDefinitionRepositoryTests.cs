using FakeItEasy;
using IResearchDefinitionRepository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ResearchDefinitionDomain.Molecule;
using ResearchDefinitionDomain.Settings;
using ResearchDefinitionRepository;
using UtilitiesServices;
using UtilityServices;

namespace MoleculesTests
{
    public sealed class ResearchDefinitionRepositoryTests
    {

        IResearchDefinitionRepo serviceToTest;

        ILogger<ResearchDefinitionRepo> logger;

        IOptions<ResearchDefinitionSettings> options;


        public ResearchDefinitionRepositoryTests()
        {
            logger = A.Fake<ILogger<ResearchDefinitionRepo>>();
            options = A.Fake<IOptions<ResearchDefinitionSettings>>();

            const string fileName = "aminoacidsidechain.yaml";

            var candidate = Path.Combine(AppContext.BaseDirectory, "Assets", fileName);

            A.CallTo(() => options.Value).Returns(new ResearchDefinitionSettings()
            {
                MoleculesLocation = Path.GetDirectoryName(candidate) ?? string.Empty
            });


            serviceToTest = new ResearchDefinitionRepo(new DirectoryServices(),
                                            new FileServices(),
                                                new YamlParser<MoleculesResearchDefinition>(),
                                                        options,
                                                            logger);

        }


        [Fact]
        public void TestParse()
        {
            var result = serviceToTest.GetMoleculesResearchDefinitions();

        }
    }
}
