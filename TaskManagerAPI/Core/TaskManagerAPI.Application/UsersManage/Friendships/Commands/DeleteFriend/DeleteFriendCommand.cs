using MediatR;
using TaskManagerAPI.Application.Dtos.UsersManage.DeleteFriend;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeleteFriend
{
	public class DeleteFriendCommand : IRequest<Unit>
	{
		public int FriendshipId { get; set; }
		public DeleteFriendDto DeleteFriendDto { get; set; }
	}
}
