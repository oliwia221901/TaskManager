using Microsoft.AspNetCore.Identity;

namespace TaskManagerAPI.Domain.Entities.UserManage
{
    public class AppUser : IdentityUser
	{
		public ICollection<Friendship> FriendRequestsSent { get; set; }
		public ICollection<Friendship> FriendRequestsReceived { get; set; }
	}
}
