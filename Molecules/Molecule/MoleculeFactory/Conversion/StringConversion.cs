using System.Globalization;
using System.Text.Json;

namespace MoleculeFactory.Conversion
{
    public static class StringConversion
    {
        public static decimal ToDecimal(string input)
        {
            if (!decimal.TryParse(input?.Replace(",", "."),
                                    NumberStyles.Float,
                                    CultureInfo.CreateSpecificCulture("en-US"),
                                    out decimal retval))
            {
                throw new ArgumentException($"Invalid input {input}");
            }
            return retval;
        }

        public static double ToDouble(string input)
        {
            if (!double.TryParse(input?.Replace(",", "."),
                                    NumberStyles.Float,
                                    CultureInfo.CreateSpecificCulture("en-US"),
                                    out double retval))
            {
                throw new ArgumentException($"Invalid input {input}");
            }
            return retval;
        }

        public static int ToInt(string input)
        {
            if (!int.TryParse(input, out int retval))
            {
                throw new ArgumentException($"Invalid input {input}");
            }
            return retval;
        }

        public static string ToString(decimal? input)
        {
            return input?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public static string ToString(decimal? input, string format)
        {
            return input?.ToString(format, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public static string ToString(double? input)
        {
            return input?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public static string ToString(double? input, string format)
        {
            return input?.ToString(format, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public static string ToString(int? input)
        {
            return input?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        }

        public static string ToString(long? input)
        {
            return input?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        }


        public static string ToJsonString<T>(T o)
        {
            return JsonSerializer.Serialize(o, JsonSerializerOptions.Default);
        }

        public static T? FromJsonString<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }

    }
}
