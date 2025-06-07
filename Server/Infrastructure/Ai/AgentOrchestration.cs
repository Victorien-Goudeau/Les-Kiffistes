using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.PromptExecution;

namespace Infrastructure.Ai;

public static class AgentOrchestration
{
    public static MagenticOrchestration Build(Kernel kernel)
    {
        var detector = IssueDetectorAgent.Create(kernel);
        var tutor    = IssueTutorAgent.CreateIssueTutor(kernel);
        var coach    = CoachAgent.Create(kernel);
        var cuisine  = CuisineAgent.Create(kernel);

        var service  = kernel.GetRequiredService<IChatCompletionService>();
        var settings = new PromptExecutionSettings();
        var manager  = new StandardMagenticManager(service, settings);

        var chat = new MagenticOrchestration(manager, new Agent[] { detector, tutor, coach, cuisine });
        return chat;
    }
}
