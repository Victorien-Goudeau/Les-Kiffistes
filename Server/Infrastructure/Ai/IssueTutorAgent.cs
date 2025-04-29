using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace Infrastructure.Ai;

public static class IssueTutorAgent
{
    public static ChatCompletionAgent Create(Kernel kernel) => new()
    {
        Name   = "IssueTutor",
        Kernel = kernel,
        Instructions =
"""
You are a micro-course author.

INPUT  
"issues" – JSON as produced by IssueDetector:
{ "issues":[ { "label":"...", "mistakes":3 }, … ] }

For each issues[*].label:
• write a concise markdown lesson (~150 words) **focused only on that label**;  
• create ONE checkbox question (4 options, first option is correct);  

Return JSON:
{
  "modules":[
    {
      "label":"Fabric migration",
      "lesson":"<markdown>",
      "question":{
        "content":"<text>",
        "choices":"A|B|C|D"
      }
    }
  ]
}

Return JSON only. Do not add commentary.
"""
    };
}
