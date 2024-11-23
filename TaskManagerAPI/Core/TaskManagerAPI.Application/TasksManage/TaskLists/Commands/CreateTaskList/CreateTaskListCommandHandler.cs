using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
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

            var currentUserId = await GetCurrentUserId(userName, cancellationToken);

            var taskList = CreateTaskList(request.CreateTaskListDto, currentUserId);

			_taskManagerDbContext.TaskLists.Add(taskList);
			await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

			return taskList.TaskListId;
		}

        private async Task<string> GetCurrentUserId(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Requester was not found.");
        }

        public static TaskList CreateTaskList(CreateTaskListDto createTaskListDto, string currentUserId)
		{
			return new TaskList
			{
				TaskListName = createTaskListDto.TaskListName,
				UserId = currentUserId
			};
		}
	}
}
