using MediatR;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommand : IRequest
	{
		public int TaskItemId { get; set; }
	}
}

