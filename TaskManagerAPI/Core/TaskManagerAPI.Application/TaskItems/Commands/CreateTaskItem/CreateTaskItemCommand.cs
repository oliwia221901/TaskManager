using MediatR;
using TaskManagerAPI.Application.Dtos.CreateTask;

namespace TaskManagerAPI.Application.TaskItems.Commands
{
    public class CreateTaskItemCommand : IRequest<int>
	{
		public CreateTaskItemDto CreateTaskItemDto { get; set; }
	}
}
