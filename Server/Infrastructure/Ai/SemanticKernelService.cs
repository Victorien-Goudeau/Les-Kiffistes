using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
namespace Infrastructure.Ai;

public static class SemanticKernelService
{
    public static void AddSemanticKernel(this IServiceCollection services,
        IConfiguration cfg)
    {
        services.AddSingleton(sp =>
        {
            var opts = cfg.GetSection("AzureOpenAI");
            var builder = Kernel.CreateBuilder();

            builder.AddAzureOpenAIChatCompletion(
                deploymentName: "gpt-4o",          // required
                endpoint:       "https://aigenstudio6832366256.openai.azure.com/",            // required
                apiKey:         "0d87619f35064fd9a6f6125d6c1bff57");             // or DefaultAzureCredential

            return builder.Build();                           // shared singleton
        });
    }
}