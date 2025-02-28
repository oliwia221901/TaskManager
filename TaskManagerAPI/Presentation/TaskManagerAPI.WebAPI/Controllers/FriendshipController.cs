﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Application.Dtos.UsersManage.CreateFriendship;
using TaskManagerAPI.Application.Dtos.UsersManage.CreateFriendships;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.AcceptFriendRequest;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeclineFriendRequest;
using TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeleteFriend;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> SendFriendRequest([FromBody] SendFriendRequestDto sendFriendRequestDto)
        {
            var command = new SendFriendRequestCommand
            {
                SendFriendRequestDto = sendFriendRequestDto
            };

            await Mediator.Send(command);
            return Ok();
        }

        [HttpPost("accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FriendshipsVm>> GetFriendships([FromRoute] int status)
        {
            var query = new GetFriendshipsQuery
            {
                Status = (FriendshipStatus)status
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("delete/{friendshipId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteFriend([FromRoute] int friendshipId)
        {
            var query = new DeleteFriendCommand
            {
                FriendshipId = friendshipId
            };

            await Mediator.Send(query);
            return NoContent();
        }
    }
}
