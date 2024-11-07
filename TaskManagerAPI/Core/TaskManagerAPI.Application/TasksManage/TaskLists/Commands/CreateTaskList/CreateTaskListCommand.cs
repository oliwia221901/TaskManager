using MediatR;
using TaskManagerAPI.Application.Dtos.CreateTask;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Commands
{
	public class CreateTaskListCommand : IRequest<int>
	{
		public CreateTaskListDto CreateTaskListDto { get; set; }
	}
}
