using System.Text.Json;
using Application.Dtos;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Domain.Interfaces;            // pour ChatHistoryExtensions

namespace Application.Services;

public sealed class RemediationLoopService
{
    private readonly AgentGroupChat _chat;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);
    private readonly IQuizRepository _quizzes;

    public RemediationLoopService(AgentGroupChat chat, IQuizRepository quizzes)
    {
        _chat = chat;
        _quizzes = quizzes;
    }

    /// <returns>Le JSON « modules » produit par IssueTutor.</returns>
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
            // On mémorise le dernier message émis par IssueTutor
#pragma warning disable SKEXP0001 // Le type est utilisé à des fins d’évaluation uniquement et est susceptible d’être modifié ou supprimé dans les futures mises à jour. Supprimez ce diagnostic pour continuer.
            if (msg.AuthorName == "IssueTutor")
                tutorMessage = msg;
#pragma warning restore SKEXP0001 // Le type est utilisé à des fins d’évaluation uniquement et est susceptible d’être modifié ou supprimé dans les futures mises à jour. Supprimez ce diagnostic pour continuer.
        }

        // La boucle s’arrête parce que la TerminationStrategy a détecté "stop"
        return tutorMessage?.Content
               ?? throw new InvalidOperationException("No module produced by IssueTutor.");
    }
}
