using MediatR;

namespace TaskManagerAPI.Application.TaskItems.Queries
{
    public class GetTaskItemForUserByIdQuery : IRequest<TaskItemForUserByIdVm>
	{
		public int TaskItemId { get; set; }
	}
}
