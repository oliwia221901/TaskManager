using TaskManagerAPI.Application.Dtos.GetTaskListForUser;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Queries.GetTaskListForUser
{
    public class TaskListForUserVm
    {
		public IEnumerable<GetTaskListForUserDto> TaskLists { get; set; }
	}
}
