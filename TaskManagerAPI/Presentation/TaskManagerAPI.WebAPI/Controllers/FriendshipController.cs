using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.CreateFriendship;
using TaskManagerAPI.Application.Dtos.CreateFriendships;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.AcceptFriendRequest;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeclineFriendRequest;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.SendFriendRequest;
using TaskManagerAPI.Application.UsersManage.Friendships.Queries.GetFriendships;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.WebAPI.Controllers
{
    [Route("api/friendships")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class FriendshipsController : BaseController
    {
        [HttpPost("send")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public async Task<ActionResult> SendFriendRequest([FromBody] SendFriendRequestDto sendFriendRequestDto)
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
        public async Task<ActionResult> AcceptFriendRequest([FromBody] AcceptFriendRequestDto acceptFriendRequestDto)
        {
            var command = new AcceptFriendRequestCommand
            {
                AcceptFriendRequestDto = acceptFriendRequestDto
            };

            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("decline")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeclineFriendRequest([FromBody] DeclineFriendRequestDto declineFriendRequestDto)
        {
            var command = new DeclineFriendRequestCommand
            {
                DeclineFriendRequestDto = declineFriendRequestDto
            };

            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<FriendshipsForUserVm>> GetFriendshipsForUser([FromRoute] int status)
        {
            var query = new GetFriendshipsForUserQuery
            {
                Status = (FriendshipStatus)status
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
