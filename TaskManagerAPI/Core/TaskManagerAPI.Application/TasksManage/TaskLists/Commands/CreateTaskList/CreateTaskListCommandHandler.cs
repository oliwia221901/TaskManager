using MediatR;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.CreateTask;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Commands
{
    public class CreateTaskListCommandHandler : IRequestHandler<CreateTaskListCommand, int>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public CreateTaskListCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
            _currentUserService = currentUserService;
		}

		public async Task<int> Handle(CreateTaskListCommand request, CancellationToken cancellationToken)
		{
            var userName = _currentUserService.GetCurrentUserName();

            var taskList = CreateTaskList(request.CreateTaskListDto, userName);

			_taskManagerDbContext.TaskLists.Add(taskList);
			await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

			return taskList.TaskListId;
		}

		public static TaskList CreateTaskList(CreateTaskListDto createTaskListDto, string username)
		{
			return new TaskList
			{
				TaskListName = createTaskListDto.TaskListName,
				UserName = username
			};
		}
	}
}
