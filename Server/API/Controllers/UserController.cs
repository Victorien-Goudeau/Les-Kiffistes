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
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetUserInfos()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { error = "User not found" });

            try
            {
                var query = new GetCurrentUserQuery(){UserId = userId};
                var userDto = await _mediator.Send(query);
                return Ok(userDto);
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