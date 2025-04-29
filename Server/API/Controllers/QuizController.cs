using Application.Dtos;
using Application.Queries;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/quiz")]
public sealed class QuizController : ControllerBase
{
    private readonly QuizApplicationService _quizService;
    private readonly IMediator _mediator;

    public QuizController(QuizApplicationService quizService, IMediator mediator)
    {
        _quizService = quizService;
        _mediator = mediator;
    }

    /// <summary>
    /// Generates a quiz for the given course.
    /// </summary>
    [HttpPost]                                         // matches POST requests :contentReference[oaicite:1]{index=1}
    [ProducesResponseType<QuizDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateAsync(
        [FromBody] string courseId, CancellationToken ct)
    {
        try
        {
            Console.WriteLine("Trying to create quiz");
            var quiz = await _quizService.CreateQuizForCourseAsync(courseId, ct);
            Console.WriteLine("Quiz created successfully");
            return Ok(quiz);                          // serialised as JSON by default :contentReference[oaicite:2]{index=2}
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine("Error in module generation : " + e.Message);
            return NotFound();
        }
        catch (ArgumentNullException)
        {
            return BadRequest("Course ID cannot be null.");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("Error in quiz generation : " + e.Message);
            return NotFound();
        }
    }

    [HttpGet("{courseId}")]
    public async Task<ActionResult<List<QuizDto>>> GetQuizByCourseId(string courseId)
    {
        try
        {
            var query = new GetCourseQuizQuery() { CourseId = courseId };
            var quiz = await _mediator.Send(query);
            return Ok(quiz);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { error = "Invalid credentials." });
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new { error = e.Message });
        }
        catch
        {
            return BadRequest(new { error = "Unknown error." });
        }
    }
    
    /// <summary>
    /// Soumet les réponses de l'utilisateur pour un quiz donné.
    /// </summary>
    [HttpPost("{quizId}/submit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitAsync(
        string quizId,
        [FromBody] SubmitQuizDto payload,
        CancellationToken ct)
    {
        if (payload == null || payload.Questions == null || !payload.Questions.Any())
        {
            return BadRequest("Le payload est invalide ou ne contient aucune question.");
        }

        try
        {
            // Appel au service d'application pour traiter la soumission
            var result = await _quizService.SubmitQuizAsync(payload, ct);
            // result peut contenir, par exemple, nombre de bonnes réponses, score, ...
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            // Quiz non trouvé
            return NotFound(new { error = e.Message });
        }
        catch (InvalidOperationException e)
        {
            // Erreur métier (ex. quiz déjà soumis, clos, etc.)
            return BadRequest(new { error = e.Message });
        }
        catch (Exception e)
        {
            // Erreur inattendue
            Console.Error.WriteLine($"Erreur lors de la soumission du quiz : {e}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Erreur serveur." });
        }
    }
}