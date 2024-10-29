using TaskManagerAPI.Application.Dtos.GetTaskListForUser;

namespace TaskManagerAPI.Application.TaskLists.Queries.GetTaskListForUser
{
    public class TaskListForUserVm
    {
		public IEnumerable<GetTaskListForUserDto> TaskLists { get; set; }
	}
}
