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

If the array is empty **or** every issues[*].mistakes < 2 → reply ONLY "stop".  
Otherwise reply ONLY "continue".

No other text.
"""
    };
}
