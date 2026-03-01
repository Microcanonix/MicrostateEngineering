namespace IUtilitiesServices
{
    public interface IYamlParser<Type>
    {
        Type Parse(string fileContent);

        string Serialize(Type entity);
    }
}
