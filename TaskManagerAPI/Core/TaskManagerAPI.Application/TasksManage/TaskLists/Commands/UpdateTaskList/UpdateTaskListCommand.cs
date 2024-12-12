using MediatR;
using TaskManagerAPI.Application.Dtos.TasksManage.UpdateTask;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Commands.UpdateTaskList
{
    public class UpdateTaskListCommand : IRequest
	{
		public int TaskListId { get; set; }
		public UpdateTaskListDto UpdateTaskListDto { get; set; }
	}
}
