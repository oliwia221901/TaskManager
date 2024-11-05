using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Commands.SendFriendRequest
{
    public class SendFriendshipVm
    {
        public bool IsSuccess { get; set; }
        public FriendshipStatus Status { get; set; }
    }
}
