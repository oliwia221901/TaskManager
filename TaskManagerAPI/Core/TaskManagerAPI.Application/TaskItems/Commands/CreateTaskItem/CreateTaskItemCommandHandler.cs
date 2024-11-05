using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.CreateTask;
using TaskManagerAPI.Domain.Entities.TaskItem;

namespace TaskManagerAPI.Application.TaskItems.Commands
{
    public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, int>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly ITaskItemAuthorizationService _taskItemAuthorizationService;

        public CreateTaskItemCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService, ITaskItemAuthorizationService taskItemAuthorizationService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
			_taskItemAuthorizationService = taskItemAuthorizationService;
		}

		public async Task<int> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
		{
            var taskItem = await CreateTaskItem(request.CreateTaskItemDto, cancellationToken);
			await _taskItemAuthorizationService.AuthorizeAccessToTaskListAsync(request.CreateTaskItemDto.TaskListId);
			_taskManagerDbContext.TaskItems.Add(taskItem);
			await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

			return taskItem.TaskItemId;
		}

		public async Task<TaskItem> CreateTaskItem(CreateTaskItemDto createTaskItemDto, CancellationToken cancellationToken)
		{
			var taskListExists = await _taskManagerDbContext.TaskLists
				.AnyAsync(t => t.TaskListId == createTaskItemDto.TaskListId, cancellationToken);

			if (!taskListExists)
				throw new NotFoundException("TaskListId was not found.");

			return new TaskItem
			{
				TaskItemName = createTaskItemDto.TaskItemName,
				TaskListId = createTaskItemDto.TaskListId
			};
		}
	}
}
