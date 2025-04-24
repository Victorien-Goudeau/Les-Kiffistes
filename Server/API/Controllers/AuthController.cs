using Application.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            command.Email = command.Email.ToLower();
            command.UserName = command.UserName.ToLower();

            if (string.IsNullOrWhiteSpace(command.Email))
                return BadRequest(new { error = "Email should not be null or with space." });
            if (string.IsNullOrWhiteSpace(command.UserName))
                return BadRequest(new { error = "User name should not be null or with space." });
            if (string.IsNullOrWhiteSpace(command.Password))
                return BadRequest(new { error = "Password should not be null or with space." });
            if (command.Role != "Student" && command.Role != "Teacher")
                return BadRequest(new { error = "Role should be Student or Teacher." });
            try
            {
                var accessToken = await _mediator.Send(command);
                return Ok(new { accessToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            command.Login = command.Login.ToLower();
            try
            {
                var accessToken = await _mediator.Send(command);
                return Ok(new { accessToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
