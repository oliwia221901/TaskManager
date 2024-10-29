using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.GetTaskItemForUserById;
using TaskManagerAPI.Domain.Entities.TaskItem;

namespace TaskManagerAPI.Application.TaskItems.Queries
{
    public class GetTaskItemForUserByIdQueryHandler : IRequestHandler<GetTaskItemForUserByIdQuery, TaskItemForUserByIdVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public GetTaskItemForUserByIdQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<TaskItemForUserByIdVm> Handle(GetTaskItemForUserByIdQuery request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();
			var taskLists = await GetTaskLists(userName, request.TaskItemId, cancellationToken);
			var taskListsDto = MapTaskListsToDto(taskLists);
			return new TaskItemForUserByIdVm { TaskLists = taskListsDto };
		}

		public async Task<List<TaskList>> GetTaskLists(string username, int taskItemId, CancellationToken cancellationToken)
		{
			var taskLists = await _taskManagerDbContext.TaskLists
				.Where(tl => tl.UserName == username)
                .Include(tl => tl.TaskItems.Where(ti => ti.TaskItemId == taskItemId))
                .ToListAsync(cancellationToken);

			return taskLists;
		}

		private static List<GetTaskListForUserByIdDto> MapTaskListsToDto(List<TaskList> taskLists)
		{
			var taskItems = taskLists
                .Where(tl => tl.TaskItems.Any())
				.Select(tk => new GetTaskListForUserByIdDto
			{
				TaskListId = tk.TaskListId,
				TaskListName = tk.TaskListName,
				UserName = tk.UserName,
				TaskItems = tk.TaskItems.Select(ti => new GetTaskItemForUserByIdDto
				{
					TaskItemId = ti.TaskItemId,
					TaskItemName = ti.TaskItemName
				}).ToList()
			}).ToList();

			return taskItems;
		}
	}
}
