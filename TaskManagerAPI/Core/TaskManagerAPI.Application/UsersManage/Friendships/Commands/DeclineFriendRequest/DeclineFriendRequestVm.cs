using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.DeclineFriendRequest
{
    public class DeclineFriendRequestVm
	{
        public bool IsSuccess { get; set; }
        public FriendshipStatus Status { get; set; }
    }
}

