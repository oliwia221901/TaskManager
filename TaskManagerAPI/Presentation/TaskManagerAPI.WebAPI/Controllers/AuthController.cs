using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.UsersManage.ChangePassword;
using TaskManagerAPI.Application.Dtos.UsersManage.LoginUser;
using TaskManagerAPI.Application.Dtos.UsersManage.RegisterUser;
using TaskManagerAPI.Application.UsersManage.ChangePassword.Commands;
using TaskManagerAPI.Application.UsersManage.LoginUser.Commands;
using TaskManagerAPI.Application.UsersManage.RegisterUser.Commands;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        [HttpPost("register")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var command = new ChangePasswordCommand
            {
                ChangePasswordDto = changePasswordDto
            };

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
