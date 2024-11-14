using MediatR;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Queries.GetFriendships
{
    public class GetFriendshipsForUserQuery : IRequest<FriendshipsForUserVm>
	{
        public FriendshipStatus Status { get; set; }
    }
}
