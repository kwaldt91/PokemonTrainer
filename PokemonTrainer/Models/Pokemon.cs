using System.Text.Json.Serialization;

namespace PokemonTrainer.Models;

public record Pokemon
{
    //test
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("weight")]
    public int? Weight { get; set; }

    [JsonPropertyName("types")]
    public List<PokemonType>? Types { get; set; }

    [JsonPropertyName("stats")]
    public List<PokemonStats> Stats { get; set; }
}
