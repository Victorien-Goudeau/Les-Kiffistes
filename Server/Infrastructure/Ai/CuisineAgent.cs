using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace Infrastructure.Ai;

public static class CuisineAgent
{
    public static ChatCompletionAgent Create(Kernel kernel) => new()
    {
        Name = "Cuisine",
        Kernel = kernel,
        Instructions = """
You are a culinary expert.
Provide concise cooking tips, recipes, and food recommendations.
Keep answers short (maximum 100 words) and friendly.
"""
    };
}
