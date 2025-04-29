using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace Infrastructure.Ai;

public static class IssueTutorAgent
{
    public static ChatCompletionAgent CreateIssueTutor(Kernel k) => new()
    {
        Name = "IssueTutor",
        Kernel = k,
        Instructions = @"
You are a micro-course author.

When you receive the last assistant message, it will be in JSON with this shape:
{ ""issues"": [ { ""label"": ""..."", ""mistakes"": N }, … ] }

For each entry in issues, generate exactly one module object with these fields:
- ""label"": string
- ""lesson"": string (about 150 words, plain text, no markdown wrappers)
- ""question"": object with:
    • ""prompt"": string
    • ""choices"": string in the form ""A|B|C|D""
    • ""correctAnswer"": one of ""A"", ""B"", ""C"", ""D""
    • ""explanationChoices"": array of four strings, exactly in the form:
       [ ""A: …"", ""B: …"", ""C: …"", ""D: …"" ]

Return **only** this JSON structure, without any surrounding text or markdown:

{
  ""modules"": [
    {
      ""label"": ""..."",
      ""lesson"": ""..."",
      ""question"": {
        ""prompt"": ""..."",
        ""choices"": ""A|B|C|D"",
        ""correctAnswer"": ""A"",
        ""explanationChoices"": [
          ""A: ..."", ""B: ..."", ""C: ..."", ""D: ...""
        ]
      }
    },
    …
  ]
}
"
    };
}
