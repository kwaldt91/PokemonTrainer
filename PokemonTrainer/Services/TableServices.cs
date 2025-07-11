using Azure.Data.Tables;
using PokemonTrainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTrainer.Services;

public class TableServices
{
    private readonly TableClient _tableClient;

    public TableServices(TableClient tableClient)
    {
        _tableClient = tableClient;
        _tableClient.CreateIfNotExists();
    }

    public async Task AddPokemonAsync(Pokemon pokemon)
    {
        PokeColum pokeColum = new PokeColum
        {
            PartitionKey = "Pokemon",
            RowKey = pokemon.Id.ToString(),
            Id = pokemon.Id,
            Name = pokemon.Name,
            Height = pokemon.Height ?? 0, // Default to 0 if null
            Weight = pokemon.Weight ?? 0, // Default to 0 if null
            Type1 = pokemon.Types.FirstOrDefault()?.Type?.Name ?? "Unknown",
            Type2 = pokemon.Types?.Skip(1).FirstOrDefault()?.Type?.Name // Nullable for second type
        };

        await _tableClient.AddEntityAsync(pokeColum);
    }
}