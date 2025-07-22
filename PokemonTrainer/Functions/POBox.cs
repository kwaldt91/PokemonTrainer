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

namespace PokemonTrainer.Functions;

public class POBox(ILogger<CatchPokemon> logger, TableServices tableServices)
{
    private readonly ILogger<CatchPokemon> _logger = logger;
    private readonly TableServices _tableServices = tableServices;
    

    [Function("RetrivePokemon")]
    public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "pokemon/retrivePokemon")] HttpRequest req)
    {
        try
        {
            var pokeList = await _tableServices.RetriveAllPokemon();

            if (pokeList == null)
            {
                _logger.LogWarning("No Pokemon found.");
            }
            else
            {
                _logger.LogInformation($"Found {pokeList.Count} Pokemon in the PO Box.");
            }

            foreach (var pokemon in pokeList)
            {
                await Task.Delay(1000);
                if (pokemon.Types.Count == 1)
                {
                    _logger.LogInformation($"Pokemon found: {pokemon.Name} and has type : {pokemon.Types[0].Type.Name}");
                }
                else
                {  
                    _logger.LogInformation($"Pokemon found: {pokemon.Name} and has types : {pokemon.Types[0].Type.Name} and : {pokemon.Types[1].Type.Name}");
                }
            }

            return new OkObjectResult(pokeList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
