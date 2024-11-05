using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.LoginUser;
using TaskManagerAPI.Application.Dtos.RegisterUser;
using TaskManagerAPI.Application.UsersManage.LoginUser;
using TaskManagerAPI.Application.UsersManage.RegisterUser;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var command = new RegisterUserCommand
            {
                RegisterUserDto = registerUserDto
            };

            var result = await Mediator.Send(command);
            return Created(string.Empty, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var command = new LoginUserCommand
            {
                LoginUserDto = loginUserDto
            };

            var token = await Mediator.Send(command);
            return Ok(new { Token = token });
        }
    }
}
