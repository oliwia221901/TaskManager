using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Features.Login;
using TaskManagerAPI.Application.Features.Register;
using TaskManagerAPI.Application.Features.Role.AddRole;
using TaskManagerAPI.Application.Features.Role.AssignRole;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : BaseController
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var command = new RegisterUserCommand
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            var result = await Mediator.Send(command);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var command = new LoginUserCommand
            {
                Username = model.Username,
                Password = model.Password
            };

            try
            {
                var token = await Mediator.Send(command);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            var command = new AddRoleCommand { Role = role };
            var result = await Mediator.Send(command);
            return Ok(new { message = result });
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var command = new AssignRoleCommand
            {
                Username = model.Username,
                Role = model.Role
            };

            var result = await Mediator.Send(command);
            return Ok(new { message = result });
        }
    }
}