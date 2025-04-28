using System.Text.Json;
using Application.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace Infrastructure.Ai;

/// <summary>
/// Génère le(s) module(s) de remédiation (markdown) pour les topics faibles,
/// au format JSON demandé par la couche Application.
/// </summary>
public sealed class RemediationService : IRemediationService
{
    /// <summary>Options de désérialisation partagées (casing strict + tolérance virgule).</summary>
    private static readonly JsonSerializerOptions _jsonOpts =
        new(JsonSerializerDefaults.Web) { AllowTrailingCommas = true };

    private readonly ChatCompletionAgent _agent;

    public RemediationService(Kernel kernel)
    {
        // Un unique agent, initialisé une fois.
        _agent = new ChatCompletionAgent
        {
            Name   = "Remediator",
            Kernel = kernel,

            // Prompt paramétré : {{$weakTopics}} sera injecté à l'exécution.
            Instructions =
"""
You are a teacher.

For each weak topic in {{$weakTopics}}, write a concise (~150 words) **markdown** lesson.
Return JSON in this exact structure:

{
  "modules": [
    { "title": "<topic>", "content": "<markdown>" }
  ]
}

Return ONLY valid JSON. Do not wrap the output in triple-backtick fences.
"""
        };
    }

    public async Task<string> BuildRemediationAsync(
        IReadOnlyList<string> weakTopics, CancellationToken ct)
    {
        // 1) Paramètres à injecter dans le template ({{$weakTopics}}).
        var args = new KernelArguments
        {
            ["weakTopics"] = JsonSerializer.Serialize(
                weakTopics,
                _jsonOpts /* vos options Web strictes » */ )
        };

        // 2) On invoque l’agent : on ne lui envoie aucun message explicite,
        //    seulement les KernelArguments via AgentInvokeOptions.
        var options = new AgentInvokeOptions { KernelArguments = args };

        await foreach (var item in _agent.InvokeAsync(
                           messages: Array.Empty<ChatMessageContent>(), // pas de chat history
                           thread:   null,
                           options:  options,
                           cancellationToken: ct))
        {
            string json = item.Message.Content;

            // 3) Validation minimale du contrat JSON :
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("modules", out _))
                throw new InvalidOperationException(
                    "Agent response missing 'modules' property.");

            // 4) On renvoie la chaîne JSON brute ; la couche Application
            //    se charge de la désérialiser en ModuleDto ou autre.
            return json;
        }

        throw new InvalidOperationException("No response from agent.");
    }

}