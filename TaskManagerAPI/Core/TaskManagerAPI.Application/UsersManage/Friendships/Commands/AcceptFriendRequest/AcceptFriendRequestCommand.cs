using MediatR;
using TaskManagerAPI.Application.Dtos.UsersManage.CreateFriendships;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommand : IRequest<AcceptFriendshipVm>
    {
        public AcceptFriendRequestDto AcceptFriendRequestDto { get; set; }
    }
}
