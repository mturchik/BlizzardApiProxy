using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace BlizzardApiProxy.Models;
public static class BlizzardConverter
{
    public static T? FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings);
    public static string ToJson<T>(this T self) => JsonConvert.SerializeObject(self, _settings);

    private static readonly JsonSerializerSettings _settings = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}

public class DisplaySlug
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("id")]
    public long? Id { get; set; }

    [JsonProperty("display_string", NullValueHandling = NullValueHandling.Ignore)]
    public string? DisplayString { get; set; }

    [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
    public string? Slug { get; set; }
}

public class TypeNamePair
{
    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}

public class IdNamePair
{
    [JsonProperty("id")]
    public long? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}
