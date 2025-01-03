using MediatR;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Queries
{
    public class GetTaskItemByIdQuery : IRequest<TaskItemByIdVm>
	{
		public int TaskItemId { get; set; }
	}
}
