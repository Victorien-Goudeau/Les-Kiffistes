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
    private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
    {
        AllowTrailingCommas = true
    };

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
                • Each question must have **one correct answer**.  
                • Concatenate the four answer choices with a "|" pipe.  
                • Embed the theme at the start of the question content using
                  the pattern **"[<theme>] <question text>"**.  
                • Do **not** output any extra fields (no theme, no answer index).

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
                      "answer": "<string>"
                    }
                  ]
                }

                Rules → 3 themes, 3 questions per theme (9 total), valid JSON only.
                """
        };
    }

    public async Task<QuizDto> GenerateQuizAsync(CourseDto course, CancellationToken ct)
    {
        var userMsg = new ChatMessageContent(
            AuthorRole.User,
            course.Content ?? string.Empty);

        _json.Converters.Add(new JsonStringEnumConverter());
        await foreach (var item in _agent.InvokeAsync(new[] { userMsg }, cancellationToken: ct))
        {
            // Le premier message retourné par l’agent contient votre JSON
            var quiz = JsonSerializer.Deserialize<QuizDto>(item.Message.Content, _json)!;
            quiz.CourseId = course.Id;
            quiz.Status   = Status.NotStarted;
            return quiz;
        }

        throw new InvalidOperationException("No response from agent");
    }
}