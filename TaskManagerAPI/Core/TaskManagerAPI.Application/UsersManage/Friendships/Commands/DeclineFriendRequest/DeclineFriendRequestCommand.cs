using MediatR;
using TaskManagerAPI.Application.Dtos.UsersManage.CreateFriendship;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeclineFriendRequest
{
    public class DeclineFriendRequestCommand : IRequest<DeclineFriendRequestVm>
	{
		public DeclineFriendRequestDto DeclineFriendRequestDto { get; set; }
	}
}
