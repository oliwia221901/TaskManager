using MediatR;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Queries
{
    public class GetTaskItemForUserByIdQuery : IRequest<TaskItemForUserByIdVm>
	{
		public int TaskItemId { get; set; }
	}
}
