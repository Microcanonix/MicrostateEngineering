
namespace IUtilitiesServices
{
    public interface IJsonParser<Type>
    {
        Type Parse(string fileContent);

        string Serialize(Type entity);

    }
}
