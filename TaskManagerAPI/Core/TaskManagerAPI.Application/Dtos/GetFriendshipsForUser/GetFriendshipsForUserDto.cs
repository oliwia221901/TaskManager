namespace TaskManagerAPI.Application.Dtos.GetFriendshipsForUser
{
    public class GetFriendshipsForUserDto
	{
		public int FriendshipId { get; set; }
        public string RequesterName { get; set; }
        public string FriendName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.ToLocalTime();
    }
}
