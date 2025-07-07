using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PokemonTrainer.Models;
using PokemonTrainer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTrainer.Functions
{
    public class PokeDex(ILogger<CatchPokemon> logger, ApiService apiService)
    {
        private readonly ILogger<CatchPokemon> _logger = logger;
        private readonly ApiService _apiService = apiService;

        [Function("ResearchPokemon")]
        public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "pokemon/research/{id}")] HttpRequest req,int id)
        {
            try
            {
                Pokemon pokemon = await _apiService.GetPokemonByIDAsync(id);

                if (pokemon == null)
                {
                    _logger.LogWarning("No Pokemon found or API call failed.");
                    return new NotFoundResult();
                }

                return new OkObjectResult(pokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
