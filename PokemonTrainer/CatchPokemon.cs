using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PokemonTrainer;

public class CatchPokemon
{
    private readonly ILogger<CatchPokemon> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public CatchPokemon(ILogger<CatchPokemon> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("PokemonApiClient");
    }

    [Function("CatchPokemon")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "pokemon/catch")] HttpRequest req)
    {
        Random random = new Random();
        int randomNumber = random.Next(1, 1303);
        var url = $"{_httpClient.BaseAddress}{randomNumber}";
        var response = await _httpClient.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
        }
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}