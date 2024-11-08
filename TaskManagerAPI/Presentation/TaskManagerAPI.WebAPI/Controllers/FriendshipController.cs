using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.CreateFriendships;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.AcceptFriendRequest;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.SendFriendRequest;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/friendships")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class FriendshipsController : BaseController
    {
        [HttpPost("send")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<IActionResult> SendFriendRequest([FromBody] SendFriendRequestDto sendFriendRequestDto)
        {
            var command = new SendFriendRequestCommand
            {
                SendFriendRequestDto = sendFriendRequestDto
            };

            var result = await Mediator.Send(command);
            return Created(string.Empty, result);
        }

        [HttpPost("accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] AcceptFriendRequestDto acceptFriendRequestDto)
        {
            var command = new AcceptFriendRequestCommand
            {
                AcceptFriendRequestDto = acceptFriendRequestDto
            };

            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
