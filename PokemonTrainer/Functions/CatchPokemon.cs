using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PokemonTrainer.Models;
using PokemonTrainer.Services;

namespace PokemonTrainer.Functions;

public class CatchPokemon(ILogger<CatchPokemon> logger, ApiService apiService, TableServices tableServices, Probability probability)
{
    private readonly ILogger<CatchPokemon> _logger = logger;
    private readonly ApiService _apiService = apiService;
    private readonly TableServices _tableServices = tableServices;
    private readonly Probability _probability = probability;

    [Function("CatchPokemon")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "pokemon/catch")] HttpRequest req)
    {
        var includeClassicParam = req.Query["classic"].ToString();
        bool includeClassic = false;

        if (!string.IsNullOrEmpty(includeClassicParam))
        {
            bool.TryParse(includeClassicParam, out includeClassic);
        }
        try
        {
            Pokemon pokemon = await _apiService.GetRandomPokemonAsync(includeClassic);
            if (pokemon == null) 
            {
                _logger.LogWarning("No Pokemon found or API call failed.");
                return new NotFoundResult();
            }

            _logger.LogInformation($"Wild Pokemon: {pokemon.Name} : has appeared!");

            var checkIfCaught = _probability.CalculateCatchChance(pokemon);

            if (checkIfCaught.Result.Equals(true))
            {
                _tableServices.AddPokemonAsync(pokemon);
            }
            return new OkResult();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }  
    }
}