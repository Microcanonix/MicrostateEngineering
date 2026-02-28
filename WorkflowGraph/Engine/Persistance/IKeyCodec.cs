namespace Engine.Persistance
{
    /// <summary>
    /// Encodes and decodes workflow node keys for persistence storage.
    /// </summary>
    public interface IKeyCodec<TKey>
        where TKey : notnull
    {
        string Encode(TKey key);

        TKey Decode(string value);
    }

    public sealed class StringKeyCodec : IKeyCodec<string>
    {
        public static StringKeyCodec Instance { get; } = new();

        public string Encode(string key) => key;

        public string Decode(string value) => value;
    }
}
