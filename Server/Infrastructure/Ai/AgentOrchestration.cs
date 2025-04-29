using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
#pragma warning disable SKEXP0110

namespace Infrastructure.Ai;

public static class AgentOrchestration
{
    public static AgentGroupChat Build(Kernel kernel)
    {
        // 1) Agents
        var detector = IssueDetectorAgent.Create(kernel);
        var tutor = IssueTutorAgent.CreateIssueTutor(kernel);
        var coach = CoachAgent.Create(kernel);

        // 2) Stratégies
        var selection = new SequentialSelectionStrategy();

        var stopFn = kernel.CreateFunctionFromPrompt("return $input == \"stop\"");
        var termination = new KernelFunctionTerminationStrategy(stopFn, kernel)
        {
            Agents = new[] { coach }, 
            MaximumIterations = 8            // borne de sécurité
        };

        // 3) GroupChat + settings (init-only → objet imbriqué)
        var chat = new AgentGroupChat(detector, tutor, coach)
        {
            ExecutionSettings = new AgentGroupChatSettings
            {
                SelectionStrategy   = selection,
                TerminationStrategy = termination
            }
        };

        return chat;
    }
}
