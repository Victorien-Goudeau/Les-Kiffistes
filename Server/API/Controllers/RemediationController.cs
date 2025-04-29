using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/remediation")]
public sealed class RemediationController : ControllerBase
{
    private readonly RemediationApplicationService remediationApplicationService;

    public RemediationController(RemediationApplicationService remediationService) =>
        remediationApplicationService = remediationService;

    /// <summary>
    /// Generates a quiz for the given course.
    /// </summary>
    [HttpPost]                                         // matches POST requests :contentReference[oaicite:1]{index=1}
    [ProducesResponseType<QuizDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateAsync(
        string quizId, CancellationToken ct)
    {
        try
        {
            var quiz = await remediationApplicationService.RemediateAsync(quizId, ct);
            return Ok(quiz);                          // serialised as JSON by default :contentReference[oaicite:2]{index=2}
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentNullException)
        {
            return BadRequest("Course ID cannot be null.");
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
