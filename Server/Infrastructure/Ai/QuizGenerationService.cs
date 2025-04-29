using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Infrastructure.Ai;

public sealed class QuizGenerationService : IQuizGenerationService
{
    private readonly ChatCompletionAgent _agent;

    public QuizGenerationService(Kernel kernel)
    {
        _agent = new ChatCompletionAgent
        {
            Name   = "QuizGen",
            Kernel = kernel,
            Instructions =
"""
You are an instructional designer.

STEP 1 – Extract exactly **3 distinct themes** from {{$course}}.

STEP 2 – For each theme, write **5 Checkbox questions** (total 15).  
• Each question has **one correct answer**.  
• Concatenate the four answer choices with a "|" pipe.  
• Prefix every question with its theme: **"[<theme>] <question text>"**.  
• Do **not** output extra fields.

Return JSON that matches *precisely* this C# schema:

{
  "id":        "<guid>",
  "title":     "<string>",
  "status":    "NotStarted",
  "courseId":  "<course-id>",
  "questions": [
    { "id":"<guid>",
      "content":"[Theme] …",
      "type":"Checkbox",
      "choices":"Choice A|Choice B|Choice C|Choice D",
      "answer":"<string>"
    }
  ]
}

Rules → 3 themes, 5 questions per theme (15 total), valid JSON only.
"""
        };
    }

    // ----------------------------------------------------------
    // Helper: build a brand-new JsonSerializerOptions each time.
    // ----------------------------------------------------------
    private static JsonSerializerOptions CreateJsonOpts()
    {
        var opts = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            AllowTrailingCommas = true
        };
        opts.Converters.Add(new JsonStringEnumConverter());   // enum-as-string
        return opts;
    }

    public async Task<QuizDto> GenerateQuizAsync(
        CourseDto course, CancellationToken ct = default)
    {
        // 1) Send course content as the user message.
        var userMsg = new ChatMessageContent(
            AuthorRole.User, course.Content ?? string.Empty);

        await foreach (var resp in _agent.InvokeAsync(
                           new[] { userMsg }, cancellationToken: ct))
        {
            // 2) Deserialize with a *fresh* options instance.
            var jsonOpts = CreateJsonOpts();
            var quiz = JsonSerializer.Deserialize<QuizDto>(
                           resp.Message.Content, jsonOpts)!;

            // 3) Final domain adjustments
            quiz.CourseId = course.Id;
            quiz.Status   = Status.NotStarted;   // enforce legal enum value
            return quiz;
        }

        throw new InvalidOperationException("No response from agent.");
    }
}
