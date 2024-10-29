namespace TaskManagerAPI.Domain.Entities.TaskItem
{
    public class TaskItem
	{
		public int TaskItemId { get; set; }
        public string TaskItemName { get; set; }

        public int TaskListId { get; set; }
		public TaskList TaskLists { get; set; }
	}
}
