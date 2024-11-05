using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.AcceptFriendRequest
{
    public class AcceptFriendshipVm
    {
        public bool IsSuccess { get; set; }
        public FriendshipStatus Status { get; set; }
    }
}
