using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace Application.Agents;

public class AssessmentCreatorAgent
{
    private readonly ChatCompletionAgent _agent;
    public string Name => _agent.Name;

    public AssessmentCreatorAgent(Kernel kernel)
    {
        _agent = new()
        {
            Name = "AssessmentCreatorAgent",
            Instructions = "Tu es un expert en création d'évaluations pédagogiques. " +
                           "Crée une évaluation complète basée sur les concepts clés du cours " +
                           "avec différents niveaux de difficulté pour tester la compréhension des élèves.",
            Kernel = kernel
        };
    }
    
    public IAsyncEnumerable<AgentResponseItem<ChatMessageContent>> InvokeAsync(
        string input, 
        AgentThread? thread = null,
        AgentInvokeOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        return _agent.InvokeAsync(input, thread, options, cancellationToken);
    }
    
    public async Task<string> GetFirstResponseAsync(
        string input, 
        CancellationToken cancellationToken = default)
    {
        await foreach(var response in _agent.InvokeAsync(input, cancellationToken: cancellationToken))
        {
            return response.Message.Content?.ToString() ?? string.Empty;
        }
        
        return string.Empty;
    }
    
    public ChatCompletionAgent GetChatCompletionAgent() => _agent;
}