using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Application.Dtos;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Domain.Interfaces;
using Microsoft.Extensions.Logging; // pour ChatHistoryExtensions

namespace Application.Services;

public sealed class RemediationLoopService
{
#pragma warning disable SKEXP0110
    private readonly MagenticOrchestration _chat;
#pragma warning restore SKEXP0110
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
    private readonly IQuizRepository _quizzes;
    private readonly ILogger<RemediationLoopService> _logger;

#pragma warning disable SKEXP0110
    public RemediationLoopService(MagenticOrchestration chat, IQuizRepository quizzes,
        ILogger<RemediationLoopService> logger)
#pragma warning restore SKEXP0110
    {
        _chat = chat;
        _quizzes = quizzes;
        _logger = logger;
    }

    /// <returns>Le JSON « modules » produit par IssueTutor.</returns>
    [Experimental("SKEXP0001")]
    public async Task<string> RunAsync(
        string quizId,
        CancellationToken ct = default)
    {
        var quiz = await _quizzes.GetQuizById(quizId);
        var answers = new List<QuestionDto>();

        foreach (var item in quiz)
        {
            var dto = new QuestionDto()
            {
                Id = item.Id,
                Answer = item.CorrectAnswers,
                Choices = item.Choices,
                Content = item.Content,
                isUserAnswerCorrectly = item.isUserAnswerCorrectly,
                Type = item.Type,
            };

            answers.Add(dto);
        }
        await _chat.ResetAsync(ct);      // on repart d’un historique vierge

        var payload = JsonSerializer.Serialize(
            answers.Select(q => new { q.Id, q.Content, q.isUserAnswerCorrectly }), _json);

        _chat.AddChatMessage(new ChatMessageContent(AuthorRole.User, payload));

        ChatMessageContent? tutorMessage = null;

        await foreach (var msg in _chat.InvokeAsync(ct))
        {
            _logger.LogInformation("[{Agent}] {Content}",
                msg.AuthorName, msg.Content);   // trace lisible

            if (msg.AuthorName.Equals("IssueTutor", StringComparison.OrdinalIgnoreCase))
                tutorMessage = msg;
        }


        // La boucle s’arrête parce que la TerminationStrategy a détecté "stop"
        return tutorMessage?.Content
               ?? throw new InvalidOperationException("No module produced by IssueTutor.");
    }
}
