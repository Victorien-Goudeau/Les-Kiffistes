using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Application.Agents;

public class CourseAnalyzerAgent
{
    private readonly ChatCompletionAgent _agent;
    public string Name => _agent.Name;

    public CourseAnalyzerAgent(Kernel kernel)
    {
        _agent = new()
        {
            Name = "CourseAnalyzerAgent",
            Instructions = "Tu es un expert en analyse pédagogique. " +
                           "Analyse ce contenu de cours et identifie les concepts clés, " +
                           "les compétences attendues et les objectifs d'apprentissage.",
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