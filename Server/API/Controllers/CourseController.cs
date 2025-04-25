using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Application.Dtos;
using System.Security.Claims;
using Application.Queries;

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
    }
}