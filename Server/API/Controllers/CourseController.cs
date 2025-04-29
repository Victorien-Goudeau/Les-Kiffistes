using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Application.Dtos;
using System.Security.Claims;
using Application.Queries;
using Application.Commands;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<List<CourseDto>>> GetAllCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Console.WriteLine($"UserId: {userId}");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { error = "User not found" });

            try
            {
                var query = new GetUserCoursesQuery(){UserId = userId};
                var Courses = await _mediator.Send(query);
                return Ok(Courses);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }
            catch
            {
                return BadRequest(new { error = "Unknown error." });
            }
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<List<CourseDto>>> CreateNewCourse([FromBody] CreateCourseCommand createCourseCommand)
        {
            try
            {
                var Course = await _mediator.Send(createCourseCommand);
                return Ok(Course);
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

        [Authorize]
        [HttpGet("{courseId}/quiz")]
        public async Task<ActionResult<List<QuizDto>>> GetQuizByCourseId(string courseId)
        {
            try
            {
                var query = new GetCourseQuizQuery(){CourseId = courseId};
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

        [Authorize]
        [HttpGet("{courseId}/modules")]
        public async Task<ActionResult<List<AIModuleDto>>> GetModulesByCourseId(string courseId)
        {
            try
            {
                var query = new GetCourseModulesQuery(){CourseId = courseId};
                var modules = await _mediator.Send(query);
                return Ok(modules);
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
    }
}