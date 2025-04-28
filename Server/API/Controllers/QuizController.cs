using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/quiz")]
public sealed class QuizController : ControllerBase
{
    private readonly QuizApplicationService _quizService;

    public QuizController(QuizApplicationService quizService) =>
        _quizService = quizService;

    /// <summary>
    /// Generates a quiz for the given course.
    /// </summary>
    [HttpPost]                                         // matches POST requests :contentReference[oaicite:1]{index=1}
    [ProducesResponseType<QuizDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateAsync(
        string courseId, CancellationToken ct)
    {
        try
        {
            var quiz = await _quizService.CreateQuizForCourseAsync(courseId, ct);
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