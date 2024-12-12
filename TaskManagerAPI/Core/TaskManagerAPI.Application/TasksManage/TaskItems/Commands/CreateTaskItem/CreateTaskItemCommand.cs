using MediatR;
using TaskManagerAPI.Application.Dtos.TasksManage.CreateTask;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands
{
    public class CreateTaskItemCommand : IRequest<int>
	{
		public int TaskListId { get; set; }
		public CreateTaskItemDto CreateTaskItemDto { get; set; }
	}
}
