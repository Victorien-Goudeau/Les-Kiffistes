using System.Diagnostics.CodeAnalysis;
using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/remediation")]
public sealed class RemediationController : ControllerBase
{
    private readonly RemediationLoopService  _remediationApplicationService;

    public RemediationController(RemediationLoopService remediationApplicationService) =>
        _remediationApplicationService = remediationApplicationService;

    /// <summary>
    /// Generates modules for the given course.
    /// </summary>
    [HttpPost]                                         // matches POST requests :contentReference[oaicite:1]{index=1}
    [ProducesResponseType<QuizDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Experimental("SKEXP0001")]
    public async Task<IActionResult> GenerateAsync(
        string quizId, CancellationToken ct)
    {
        try 
        {
            var modules = await _remediationApplicationService.RunAsync(quizId, ct);
            return Ok(modules);                          // serialised as JSON by default :contentReference[oaicite:2]{index=2}
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentNullException)
        {
            return BadRequest("Course ID cannot be null.");
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }
}
