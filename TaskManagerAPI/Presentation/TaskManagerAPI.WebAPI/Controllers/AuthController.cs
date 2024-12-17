using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.UsersManage.ChangePassword;
using TaskManagerAPI.Application.Dtos.UsersManage.LoginUser;
using TaskManagerAPI.Application.Dtos.UsersManage.RegisterUser;
using TaskManagerAPI.Application.UsersManage.LoginUser;
using TaskManagerAPI.Application.UsersManage.RegisterUser;
using TaskManagerAPI.Application.UsersManage.ResetPassword;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        [HttpPost("register")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var command = new RegisterUserCommand
            {
                RegisterUserDto = registerUserDto
            };

            var result = await Mediator.Send(command);
            return Created(string.Empty, result);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var command = new LoginUserCommand
            {
                LoginUserDto = loginUserDto
            };

            var token = await Mediator.Send(command);
            return Ok(new { Token = token });
        }

        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ResetPassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var command = new ChangePasswordCommand
            {
                ChangePasswordDto = changePasswordDto
            };

            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
