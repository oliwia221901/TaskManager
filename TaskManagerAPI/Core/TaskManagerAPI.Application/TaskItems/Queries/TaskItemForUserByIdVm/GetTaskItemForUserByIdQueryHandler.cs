using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.GetTaskItemForUserById;
using TaskManagerAPI.Domain.Entities.TaskItem;

namespace TaskManagerAPI.Application.TaskItems.Queries
{
    public class GetTaskItemForUserByIdQueryHandler : IRequestHandler<GetTaskItemForUserByIdQuery, TaskItemForUserByIdVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly ITaskItemAuthorizationService _taskItemAuthorizationService;

		public GetTaskItemForUserByIdQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService, ITaskItemAuthorizationService taskItemAuthorizationService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
			_taskItemAuthorizationService = taskItemAuthorizationService;
		}

		public async Task<TaskItemForUserByIdVm> Handle(GetTaskItemForUserByIdQuery request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();
			var taskLists = await GetTaskListByUserName(userName, request.TaskItemId, cancellationToken);
            await _taskItemAuthorizationService.AuthorizeAccessToTaskItemAsync(request.TaskItemId);
            var taskListsDto = MapTaskListsToDto(taskLists, request.TaskItemId);
			return new TaskItemForUserByIdVm { TaskLists = taskListsDto };
		}

        public async Task<List<TaskList>> GetTaskListByUserName(string username, int taskItemId, CancellationToken cancellationToken)
        {
            var taskItemExists = await _taskManagerDbContext.TaskItems
                .AnyAsync(ti => ti.TaskItemId == taskItemId, cancellationToken);

            if (!taskItemExists)
                throw new NotFoundException($"TaskItemId {taskItemId} was not found.");

            var taskLists = await _taskManagerDbContext.TaskLists
                .Where(tl => tl.UserName == username &&
                             tl.TaskItems.Any(ti => ti.TaskItemId == taskItemId))
                .Include(tl => tl.TaskItems)
                .ToListAsync(cancellationToken);

            return taskLists;
        }

        private static List<GetTaskListForUserByIdDto> MapTaskListsToDto(List<TaskList> taskLists, int taskItemId)
		{
			var taskItems = taskLists
                .Where(tl => tl.TaskItems.Any())
				.Select(tk => new GetTaskListForUserByIdDto
				{
					TaskListId = tk.TaskListId,
					TaskListName = tk.TaskListName,
					UserName = tk.UserName,
					TaskItems = tk.TaskItems
                        .Where(ti => ti.TaskItemId == taskItemId)
                        .Select(ti => new GetTaskItemForUserByIdDto
						{
							TaskItemId = ti.TaskItemId,
							TaskItemName = ti.TaskItemName
						}).ToList()
				}).ToList();

			return taskItems;
		}
	}
}
