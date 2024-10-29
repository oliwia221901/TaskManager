using TaskManagerAPI.Application.Dtos.GetTaskListForUser;

namespace TaskManagerAPI.Application.Dtos.GetTaskListForUser
{
    public class GetTaskListForUserDto
    {
		public int TaskListId { get; set; }
		public string TaskListName { get; set; }
		public string UserName { get; set; }

		public IEnumerable<GetTaskItemForUserDto> TaskItems { get; set; }
	}
}
