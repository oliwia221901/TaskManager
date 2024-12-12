using TaskManagerAPI.Application.Dtos.TasksManage.GetTaskList;

namespace TaskManagerAPI.Application.Dtos.GetTaskList
{
    public class GetTaskListDto
    {
		public int TaskListId { get; set; }
		public string TaskListName { get; set; }
		public string UserName { get; set; }

		public IEnumerable<GetTaskItemDto> TaskItems { get; set; }
	}
}
