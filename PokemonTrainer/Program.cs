using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonTrainer.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

builder.Services.AddHttpClient("PokemonApiClient", client =>
{
    client.BaseAddress = new Uri(configuration["PokeApiBaseUrl"]);
});

builder.Services.AddSingleton<ApiService>();

builder.Build().Run();
