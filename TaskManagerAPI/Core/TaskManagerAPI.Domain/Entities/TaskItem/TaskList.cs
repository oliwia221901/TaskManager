namespace TaskManagerAPI.Domain.Entities.TaskItem
{
    public class TaskList
	{
		public int TaskListId { get; set; }
		public string TaskListName { get; set; }

		public IEnumerable<TaskItem> TaskItems { get; set; }

        public string UserName { get; set; }
    }
}
