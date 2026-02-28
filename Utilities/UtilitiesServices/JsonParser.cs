using IUtilitiesServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UtilityServices
{
    public sealed class JsonParser<Type> : IJsonParser<Type>
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public JsonParser()
        {
            _options.Converters.Add(new JsonStringEnumConverter());
        }

        public Type Parse(string fileContent)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(fileContent);
            return JsonSerializer.Deserialize<Type>(fileContent, _options)!;
        }

        public string Serialize(Type entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return JsonSerializer.Serialize(entity, _options);
        }
    }
}
