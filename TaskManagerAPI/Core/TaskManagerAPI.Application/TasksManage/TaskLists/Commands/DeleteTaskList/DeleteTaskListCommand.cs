using MediatR;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Commands.DeleteTaskList
{
    public class DeleteTaskListCommand : IRequest
	{
		public int TaskListId { get; set; }
	}
}
