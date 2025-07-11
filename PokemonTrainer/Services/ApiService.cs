using PokemonTrainer.Models;
using System.Text.Json;

namespace PokemonTrainer.Services;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("PokemonApiClient");
    }
    public async Task<Pokemon> GetRandomPokemonAsync()
    {
        Random random = new Random();
        int randomNumber = random.Next(1, 1026);

        var url = $"{_httpClient.BaseAddress}{randomNumber}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var responseData = response.Content.ReadAsStringAsync();

            if (responseData != null)
            {
                var pokemon = JsonSerializer.Deserialize<Pokemon>(await responseData);

                return pokemon;
            }
        }

        return null;
    }

    public async Task<Pokemon> GetPokemonByIDAsync(int id)
    {
        var url = $"{_httpClient.BaseAddress}{id}";
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            if (responseData != null)
            {
                var pokemon = JsonSerializer.Deserialize<Pokemon>(responseData);
                //for uncaught pokemon, set properties to null
                pokemon.Height = null;
                pokemon.Weight = null;
                pokemon.Types = null;
                return pokemon;
            }
        }
        return null;
    }
}
