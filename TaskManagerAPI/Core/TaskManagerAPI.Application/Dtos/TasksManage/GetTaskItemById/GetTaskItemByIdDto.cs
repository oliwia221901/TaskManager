namespace TaskManagerAPI.Application.Dtos.TasksManage.GetTaskItemById
{
	public class GetTaskItemByIdDto
    {
		public int TaskItemId { get; set; }
		public string TaskItemName { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastModifiedByUser { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
