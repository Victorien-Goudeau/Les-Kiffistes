using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace Infrastructure.Ai;

// Infrastructure/Ai/AgentFactory.cs
public static class AgentFactory
{
    public static ChatCompletionAgent CreateQuizAgent(Kernel k) => new()
    {
        Name = "QuizGenerator",
        Kernel = k,
        Instructions =
            """
            You are an instructional designer.
            Convert {{$course}} into a JSON quiz with five questions that matches this C# schema:
            { "id": "...", "title": "...", "status": "Pending", "courseId": "...",
              "questions":[{ "id":"...", "content":"...", "type":"MultipleChoice",
                             "choices":["...","..."]}] }
            Return **only** valid JSON.
            """
    };

    public static ChatCompletionAgent CreateGapAgent(Kernel k) => new()
    {
        Name = "GapDetector",
        Kernel = k,
        Instructions =
            """
            Input quiz: {{$quiz}}
            Input scores: {{$scores}}
            Return a JSON array of weak topic names.
            """
    };

    public static ChatCompletionAgent CreateRemediationAgent(Kernel k) => new()
    {
        Name = "Remediator",
        Kernel = k,
        Instructions =
            """
            For each topic in {{$weakTopics}}, write a 150-word markdown lesson and
            wrap everything exactly like:
            { "modules":[ { "title":"<topic>", "content":"<md>" } ] }
            Return JSON only.
            """
    };
}
