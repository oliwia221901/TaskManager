using TaskManagerAPI.Application.Dtos.GetTaskList;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Queries.GetTaskList
{
    public class TaskListVm
    {
		public IEnumerable<GetTaskListDto> TaskLists { get; set; }
	}
}
