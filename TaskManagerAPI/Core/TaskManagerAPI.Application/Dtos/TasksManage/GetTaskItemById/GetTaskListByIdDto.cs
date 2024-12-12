namespace TaskManagerAPI.Application.Dtos.TasksManage.GetTaskItemById
{
    public class GetTaskListByIdDto
    {
		public int TaskListId { get; set; }
		public string TaskListName { get; set; }
		public string UserName { get; set; }

		public IEnumerable<GetTaskItemByIdDto> TaskItems { get; set; }
	}
}
