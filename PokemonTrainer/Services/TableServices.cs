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

    public async Task<List<Pokemon>> RetriveAllPokemon()
    {
        List<Pokemon> pokemonList = new List<Pokemon>();

        var query = _tableClient.QueryAsync<PokeColum>();
        
        await foreach (var entity in query)
        {
            Pokemon pokemon = new Pokemon
            {
                Id = entity.Id,
                Name = entity.Name,
                Height = entity.Height,
                Weight = entity.Weight,
                Types = new List<PokemonType>
                {
                    new PokemonType { 
                        Type = new Models.Type
                        {
                            Name = entity.Type1
                        }
                    }
                }
            };

            if (entity.Type2 != null)
            {
                pokemon.Types.Add(

                    new PokemonType
                    {
                        Type = new Models.Type
                        {
                            Name = entity.Type2
                        }
                    }
                );
            }

            pokemonList.Add(pokemon);
        }

        return pokemonList;
    }
}