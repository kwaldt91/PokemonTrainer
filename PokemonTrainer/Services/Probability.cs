using Microsoft.Extensions.Logging;
using PokemonTrainer.Models;

namespace PokemonTrainer.Services;

public class Probability(ILogger<Probability> logger)
{
    private readonly ILogger<Probability> _logger = logger;
    int timeDelay = 3000;

    public async Task<bool> CalculateCatchChance(Pokemon pokemon)
    {
        // Calculate the catch chance based on the total stats of the Pokemon
        float a = 0.03f; // Example value, adjust for difficulty
        float b = 450f;  // Example value, adjust for balance

        int pokeStatTotal = pokemon.Stats.Sum(stat => stat.BaseStat);
        float catchRateNew = 100f / (1f + (float)Math.Exp(a * (pokeStatTotal - b)));

        if(catchRateNew < 1.00f)
        {
            catchRateNew = 1; //min catch rate
        }

        int attemptsLeft = 3;
        _logger.LogInformation($"You have a {catchRateNew}% chance to catch them - attepmts left = {attemptsLeft}");

        await Task.Delay(timeDelay);

        while (attemptsLeft > 0)
        {
            int randomChance = new Random().Next(1, 101);
            _logger.LogInformation($"Attempting to catch {pokemon.Name}");
            await Task.Delay(timeDelay);
            if (randomChance <= catchRateNew)
            {
                _logger.LogInformation($"You caught {pokemon.Name}!");
                return true;
            }
            else
            {
                attemptsLeft--;
                _logger.LogInformation($"Failed to catch {pokemon.Name}. Attempts left: {attemptsLeft}");
                await Task.Delay(timeDelay);
                if (attemptsLeft == 0)
                {
                    _logger.LogInformation($"You failed to catch {pokemon.Name}. It has escaped!");
                    pokemon.Height = null; // Set properties to null for uncaught Pokemon
                    pokemon.Weight = null;
                    pokemon.Types = null;

                    return false;
                }
            }
        }
        return false;
    }
}
