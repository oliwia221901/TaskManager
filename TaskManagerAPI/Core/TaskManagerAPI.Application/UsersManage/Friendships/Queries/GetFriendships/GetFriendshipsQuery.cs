using MediatR;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Queries.GetFriendships
{
    public class GetFriendshipsQuery : IRequest<FriendshipsVm>
	{
        public FriendshipStatus Status { get; set; }
    }
}
