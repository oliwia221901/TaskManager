using TaskManagerAPI.Application.Dtos.GetFriendshipsForUser;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Queries.GetFriendships
{
    public class FriendshipsForUserVm
	{
		public IEnumerable<GetFriendshipsForUserDto> Friendships { get; set; }
	}
}
