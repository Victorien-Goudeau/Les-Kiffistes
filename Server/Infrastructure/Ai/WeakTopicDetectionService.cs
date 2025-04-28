using System.Text.Json;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Infrastructure.Ai;

public sealed class WeakTopicDetectionService : IWeakTopicDetectionService
{
    private readonly ChatCompletionAgent _agent;
    private static readonly JsonSerializerOptions _json =
        new(JsonSerializerDefaults.Web) { AllowTrailingCommas = true };

    public WeakTopicDetectionService(Kernel kernel)
    {
        _agent = new ChatCompletionAgent
        {
            Name   = "GapDetector",
            Kernel = kernel,
            Instructions =
                """
                You are an evaluator.

                Input "quiz"  : {{$quiz}}
                Input "scores": {{$scores}}   // JSON { "<questionId>": <0–1> }

                Return a JSON array of the module titles (strings) where
                the learner performed poorly (score < 0.6).
                Return ONLY the JSON array.
                """
        };
    }

    public async Task<IReadOnlyList<string>> DetectWeakTopicsAsync(
        QuizDto quiz, IDictionary<string,double> scores, CancellationToken ct)
    {
        var userMsg = new ChatMessageContent(
            AuthorRole.User,
            JsonSerializer.Serialize(new { quiz, scores }, _json));

        await foreach (var resp in _agent.InvokeAsync(new[] { userMsg }, cancellationToken: ct))
        {
            return JsonSerializer.Deserialize<List<string>>(resp.Message.Content, _json)!;
        }
        return Array.Empty<string>();
    }

}