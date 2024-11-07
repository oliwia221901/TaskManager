using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.CreateTask;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands
{
    public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, int>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ITaskAuthorizationService _taskAuthorizationService;

        public CreateTaskItemCommandHandler(ITaskManagerDbContext taskManagerDbContext, ITaskAuthorizationService taskAuthorizationService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_taskAuthorizationService = taskAuthorizationService;
		}

		public async Task<int> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
		{
            var taskItem = await CreateTaskItem(request.CreateTaskItemDto, cancellationToken);

			await _taskAuthorizationService.AuthorizeAccessToTaskList(request.CreateTaskItemDto.TaskListId);

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
