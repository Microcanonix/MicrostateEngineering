using System.Text.Json;

namespace Engine.Workflow
{
    /// <summary>
    /// Provides mutable, per-run data that workflow nodes can use to exchange state.
    /// </summary>
    public sealed class WorkflowContext
    {
        private readonly Dictionary<string, object?> _values = new(StringComparer.Ordinal);

        /// <summary>
        /// Stores a value under the specified key.
        /// </summary>
        public void Set(string key, object? value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);
            _values[key] = value;
        }

        /// <summary>
        /// Replaces all values from a persisted JSON context map.
        /// </summary>
        public void Restore(IReadOnlyDictionary<string, JsonElement> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            _values.Clear();
            foreach (var entry in values)
            {
                _values[entry.Key] = entry.Value;
            }
        }

        /// <summary>
        /// Exports context values as JSON elements suitable for persistence.
        /// </summary>
        public IReadOnlyDictionary<string, JsonElement> ToJsonMap(JsonSerializerOptions? options = null)
        {
            var map = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
            foreach (var entry in _values)
            {
                map[entry.Key] = entry.Value is JsonElement json
                    ? json
                    : JsonSerializer.SerializeToElement(entry.Value, entry.Value?.GetType() ?? typeof(object), options);
            }

            return map;
        }

        /// <summary>
        /// Attempts to read a strongly typed value from the context.
        /// </summary>
        public bool TryGet<TValue>(string key, out TValue? value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);
            if (_values.TryGetValue(key, out var raw))
            {
                if (raw is TValue cast)
                {
                    value = cast;
                    return true;
                }

                if (raw is JsonElement json)
                {
                    try
                    {
                        value = json.Deserialize<TValue>();
                        return true;
                    }
                    catch
                    {
                    }
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Determines whether a key exists in the context.
        /// </summary>
        public bool Contains(string key)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);
            return _values.ContainsKey(key);
        }
    }
}
