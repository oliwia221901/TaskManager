using TaskManagerAPI.Application.Dtos.UsersManage.GetFriendships;

namespace TaskManagerAPI.Application.UsersManage.Friendships.Queries.GetFriendships
{
    public class FriendshipsVm
	{
		public IEnumerable<GetFriendshipsDto> Friendships { get; set; }
	}
}
