using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;

public class Unserialize
{
    public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson<T>(T self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}
