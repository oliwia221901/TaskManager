namespace TaskManagerAPI.Application.Dtos.GetTaskItemForUserById
{
    public class GetTaskListForUserByIdDto
    {
		public int TaskListId { get; set; }
		public string TaskListName { get; set; }
		public string UserName { get; set; }

		public IEnumerable<GetTaskItemForUserByIdDto> TaskItems { get; set; }
	}
}
