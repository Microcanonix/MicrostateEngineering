using IUtilitiesServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace UtilitiesServices
{
    public sealed class YamlParser<Type> : IYamlParser<Type>
    {
        public Type Parse(string fileContent)
        {
            var deserializer = new DeserializerBuilder()
                                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                        .Build();

            return deserializer.Deserialize<Type>(fileContent);
        }

        public string Serialize(Type entity)
        {
            var serializer = new SerializerBuilder()
                                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                        .Build();

            return serializer.Serialize(entity);
        }
    }
}
