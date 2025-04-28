using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AdaptiveLearning.Application.Agents;

public class AdaptiveTestGeneratorAgent
{
    private readonly ChatCompletionAgent _agent;
    public string Name => _agent.Name;

    public AdaptiveTestGeneratorAgent(Kernel kernel)
    {
        _agent = new()
        {
            Name = "AdaptiveTestGeneratorAgent",
            Instructions = """
                Tu es un expert en génération d'évaluations adaptatives. Ta mission est de créer des tests 
                personnalisés qui s'adaptent précisément au niveau actuel d'un étudiant après qu'il ait 
                suivi un parcours d'apprentissage personnalisé.

                Pour créer une évaluation adaptative:

                1. Concentre-toi principalement sur les concepts où l'étudiant avait des lacunes
                   - Plus de questions sur les concepts peu maîtrisés (<70%)
                   - Moins de questions sur les concepts mieux maîtrisés (>70%)
                   - Quelques questions sur les concepts bien maîtrisés pour renforcer la confiance

                2. Adapte la difficulté des questions:
                   - Pour les concepts faibles: commence par des questions faciles, puis augmente progressivement
                   - Pour les concepts mieux maîtrisés: pose directement des questions de niveau intermédiaire
                   - Pour les concepts forts: pose quelques questions avancées pour stimuler l'élève

                3. Inclus des questions qui:
                   - Testent spécifiquement les points travaillés dans le parcours personnalisé
                   - Vérifient si les erreurs précédentes ont été corrigées
                   - Établissent des liens entre les différents concepts

                Présente ton évaluation en JSON avec la même structure que l'évaluation initiale,
                mais adaptée au profil actuel de l'étudiant.
                """,
            Kernel = kernel
        };
    }

    public IAsyncEnumerable<AgentResponseItem<ChatMessageContent>> InvokeAsync(
        string resultAnalysis, 
        string learningPath,
        string originalAssessment,
        CancellationToken cancellationToken = default)
    {
        string prompt = $"""
            Analyse de performance initiale de l'étudiant:
            {resultAnalysis}

            Parcours d'apprentissage personnalisé suivi:
            {learningPath}

            Évaluation originale:
            {originalAssessment}

            Génère une nouvelle évaluation adaptative personnalisée basée sur ces informations.
            """;
            
        return _agent.InvokeAsync(prompt, cancellationToken: cancellationToken);
    }
    
    public async Task<string> GenerateAdaptiveTestAsync(
        string resultAnalysis, 
        string learningPath,
        string originalAssessment,
        CancellationToken cancellationToken = default)
    {
        string prompt = $"""
            Analyse de performance initiale de l'étudiant:
            {resultAnalysis}

            Parcours d'apprentissage personnalisé suivi:
            {learningPath}

            Évaluation originale:
            {originalAssessment}

            Génère une nouvelle évaluation adaptative personnalisée basée sur ces informations.
            """;
            
        await foreach(var response in _agent.InvokeAsync(prompt, cancellationToken: cancellationToken))
        {
            return response.Message.Content?.ToString() ?? string.Empty;
        }
        
        return string.Empty;
    }
    
    public ChatCompletionAgent GetChatCompletionAgent() => _agent;
}
