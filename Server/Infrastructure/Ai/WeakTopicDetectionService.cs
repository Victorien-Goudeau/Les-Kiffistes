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
            You are a learning-analytics evaluator.

            INPUT  
            • **answers** – an array of QUESTION objects, each with:  
              { "id":"<guid>",  
                "content":"[<theme>] <question text>",  
                "isUserAnswerCorrectly": <true|false> }

            TASK  
            1. From every question, extract the text inside the square brackets – that is the raw *theme*.  
            2. Convert each raw theme to an **extra-precise label** that meets *all* of these rules:  it's just an example !!
               • no more than 3 words • only nouns/keywords (no verbs) • no generic terms such as “Business”, “Data”, “Events”.  
               – Good ↦ “Fabric migration”, “Power BI”, “Generative AI”  
               – Bad  ↦ “Cloud Solutions”, “Team events”.  
            3. Count, per precise label, how many questions were answered incorrectly (`isUserAnswerCorrectly == false`).  
            4. Select the labels with **two or more** wrong answers.  
            5. Return a JSON **array of those labels**, unique and sorted from most to least wrong answers.  
            6. Output JSON *only* – no markdown, no extra keys, no explanation text.
            
            """
        };
    }

    public async Task<IReadOnlyList<string>> DetectWeakTopicsAsync(
    List<QuestionDto> questions, CancellationToken ct)
    {
        var answersJson = JsonSerializer.Serialize(
            questions.Select(q => new {
                q.Id,
                q.Content,
                q.isUserAnswerCorrectly
            }), _json);

        var userMsg = new ChatMessageContent(
            AuthorRole.User,
            $"=== ANSWERS JSON START ===\n{answersJson}\n=== ANSWERS JSON END ===");

        await foreach (var resp in _agent.InvokeAsync(
                           new[] { userMsg },      // 👈 message utilisateur
                           cancellationToken: ct))
        {
            return JsonSerializer.Deserialize<List<string>>(
                       resp.Message.Content, _json)!.AsReadOnly();
        }
        return Array.Empty<string>();
    }


}