using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace Application.Agents;

public class ResultAnalyzerAgent
{
    private readonly ChatCompletionAgent _agent;
    public string Name => _agent.Name;

    public ResultAnalyzerAgent(Kernel kernel)
    {
        _agent = new()
        {
            Name = "ResultAnalyzerAgent",
            Instructions = "Tu es un expert en analyse de performances d'apprentissage. " +
                           "Analyse les résultats de cette évaluation pour identifier " +
                           "les forces et faiblesses de l'élève sur chaque concept.",
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