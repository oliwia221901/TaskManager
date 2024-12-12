using MediatR;
using TaskManagerAPI.Application.Dtos.UsersManage.CreateFriendships;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.SendFriendRequest
{
    public class SendFriendRequestCommand : IRequest<SendFriendshipVm>
    {
        public SendFriendRequestDto SendFriendRequestDto { get; set; }
    }
}
