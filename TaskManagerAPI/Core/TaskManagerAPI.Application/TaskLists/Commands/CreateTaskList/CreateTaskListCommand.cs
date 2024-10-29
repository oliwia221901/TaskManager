using MediatR;
using TaskManagerAPI.Application.Dtos.CreateTask;

namespace TaskManagerAPI.Application.TaskLists.Commands
{
	public class CreateTaskListCommand : IRequest<int>
	{
		public CreateTaskListDto CreateTaskListDto { get; set; }
	}
}
