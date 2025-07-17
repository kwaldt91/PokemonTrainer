using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PokemonTrainer.Models;
using PokemonTrainer.Services;

namespace PokemonTrainer.Functions;

public class TestCatchByID(ILogger<TestCatchByID> logger, ApiService apiService, Probability probability)
{
    private readonly ILogger<TestCatchByID> _logger = logger;
    private readonly ApiService _apiService = apiService;
    private readonly Probability _probability = probability;
    // This function is for testing purposes only and should not be used in production.
    // It retrieves a Pokemon by its ID from the API and returns it.

    [Function("CatchPokemonById-TestingOnly")]
    public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "pokemon/PickCatchTestingOnly/{id}")] HttpRequest req, int id)
    {
        try
        {
            Pokemon pokemon = await _apiService.TestingOnlySelectPokemonAsync(id);

            if (pokemon == null)
            {
                _logger.LogWarning("No Pokemon found or API call failed.");
                return new NotFoundResult();
            }

            _logger.LogInformation($"Wild Pokemon: {pokemon.Name} : has appeared!");

            var checkIfCaught = await _probability.CalculateCatchChance(pokemon);//dont want to catch it, just want to test probability

            return new OkObjectResult(pokemon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
