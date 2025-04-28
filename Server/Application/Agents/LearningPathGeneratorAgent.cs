using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AdaptiveLearning.Application.Agents;

public class LearningPathGeneratorAgent
{
    private readonly ChatCompletionAgent _agent;
    public string Name => _agent.Name;

    public LearningPathGeneratorAgent(Kernel kernel)
    {
        _agent = new()
        {
            Name = "LearningPathGeneratorAgent",
            Instructions = """
                Tu es un expert en conception de parcours d'apprentissage personnalisés. Ta mission est de créer
                des modules d'apprentissage adaptés spécifiquement aux lacunes identifiées chez un étudiant.

                Pour chaque concept que l'étudiant ne maîtrise pas suffisamment (<70% de maîtrise), génère:

                1. Un mini-module d'apprentissage contenant:
                   - Une explication claire et simplifiée du concept
                   - Des exemples concrets et pertinents
                   - Des illustrations ou analogies facilitant la compréhension
                   - Des points de vérification pour s'assurer de la compréhension
                   - Des exercices progressifs (du plus simple au plus complexe)

                2. Adapte ton approche pédagogique en fonction:
                   - Du niveau actuel de l'étudiant sur ce concept
                   - Des erreurs spécifiques qu'il a commises
                   - Des relations avec d'autres concepts qu'il maîtrise mieux

                Structure ta réponse en JSON avec pour chaque concept:
                - Le nom du concept
                - Le niveau de maîtrise actuel
                - Le contenu du module d'apprentissage
                - Les exercices recommandés
                - Les points de vérification
                """,
            Kernel = kernel
        };
    }

    public IAsyncEnumerable<AgentResponseItem<ChatMessageContent>> InvokeAsync(
        string resultAnalysis, 
        string courseContent,
        CancellationToken cancellationToken = default)
    {
        string prompt = $"""
            Analyse de performance de l'étudiant:
            {resultAnalysis}

            Contenu original du cours:
            {courseContent}

            Génère un parcours d'apprentissage personnalisé basé sur cette analyse.
            """;
            
        return _agent.InvokeAsync(prompt, cancellationToken: cancellationToken);
    }
    
    public async Task<string> GenerateLearningPathAsync(
        string resultAnalysis, 
        string courseContent,
        CancellationToken cancellationToken = default)
    {
        string prompt = $"""
            Analyse de performance de l'étudiant:
            {resultAnalysis}

            Contenu original du cours:
            {courseContent}

            Génère un parcours d'apprentissage personnalisé basé sur cette analyse.
            """;
            
        await foreach(var response in _agent.InvokeAsync(prompt, cancellationToken: cancellationToken))
        {
            return response.Message.Content?.ToString() ?? string.Empty;
        }
        
        return string.Empty;
    }
    
    public ChatCompletionAgent GetChatCompletionAgent() => _agent;
}
