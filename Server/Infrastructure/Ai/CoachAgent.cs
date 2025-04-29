using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace Infrastructure.Ai;

public static class CoachAgent
{
    public static ChatCompletionAgent Create(Kernel kernel) => new()
    {
        Name   = "Coach",
        Kernel = kernel,
        Instructions =
"""
You are a supervisor.

INPUT  
"issues" – JSON array from IssueDetector.

If the last assistant message is an empty array OR every issues[*].mistakes<2
reply "stop"; else "continue".

No other text.
"""
    };
}
