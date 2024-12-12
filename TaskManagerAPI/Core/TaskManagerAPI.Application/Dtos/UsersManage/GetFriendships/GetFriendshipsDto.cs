namespace TaskManagerAPI.Application.Dtos.UsersManage.GetFriendships
{
    public class GetFriendshipsDto
	{
		public int FriendshipId { get; set; }
        public string RequesterName { get; set; }
        public string FriendName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
