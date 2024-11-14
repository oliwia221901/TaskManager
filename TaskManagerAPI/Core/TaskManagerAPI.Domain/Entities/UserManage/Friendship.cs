using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Domain.Entities.UserManage
{
    public class Friendship
	{
		public int FriendshipId { get; set; }

        public string RequesterId { get; set; }
		public AppUser Requester { get; set; }

        public string FriendId { get; set; }
		public AppUser Friend { get; set; }

		public FriendshipStatus Status { get; set; } = FriendshipStatus.NoInteraction;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow.ToLocalTime();

	}
}
