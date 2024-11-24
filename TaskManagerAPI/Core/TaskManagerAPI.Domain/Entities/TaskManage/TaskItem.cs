namespace TaskManagerAPI.Domain.Entities.TaskManage
{
    public class TaskItem
	{
		public int TaskItemId { get; set; }
        public string TaskItemName { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastModifiedByUser { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int TaskListId { get; set; }
		public TaskList TaskLists { get; set; }
	}
}
