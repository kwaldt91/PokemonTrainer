using System.Text.Json.Serialization;

namespace PokemonTrainer.Models;

public record PokemonType
{
    [JsonPropertyName("slot")]
    public int Slot { get; set; }

    [JsonPropertyName("type")]
    public Type Type { get; set; }
}

public record Type
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
}
