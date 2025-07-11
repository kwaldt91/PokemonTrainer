using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PokemonTrainer.Models;
using PokemonTrainer.Services;

namespace PokemonTrainer.Functions;

public class CatchPokemon(ILogger<CatchPokemon> logger, ApiService apiService, TableServices tableServices)
{
    private readonly ILogger<CatchPokemon> _logger = logger;
    private readonly ApiService _apiService = apiService;
    private readonly TableServices _tableServices = tableServices;

    [Function("CatchPokemon")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "pokemon/catch")] HttpRequest req)
    {
        try
        {
            Pokemon pokemon = await _apiService.GetRandomPokemonAsync();

            if (pokemon == null) 
            {
                _logger.LogWarning("No Pokemon found or API call failed.");
                return new NotFoundResult();
            }

            _tableServices.AddPokemonAsync(pokemon);
            return new OkObjectResult(pokemon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }  
    }
}