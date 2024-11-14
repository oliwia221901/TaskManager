using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.GetTaskListForUser;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Queries.GetTaskListForUser
{
    public class GetTaskListForUserQueryHandler : IRequestHandler<GetTaskListForUserQuery, TaskListForUserVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public GetTaskListForUserQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<TaskListForUserVm> Handle(GetTaskListForUserQuery request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();
			var taskLists = await GetTaskListByUserName(userName, cancellationToken);
			var taskListsDto = MapTaskListByIdToDto(taskLists);

            return new TaskListForUserVm
			{
				TaskLists = taskListsDto
			};
        }

		public async Task<List<TaskList>> GetTaskListByUserName(string username, CancellationToken cancellationToken)
		{
			var taskListByUserName = await _taskManagerDbContext.TaskLists
				.Where(tl => tl.UserName == username)
				.Include(tl => tl.TaskItems)
				.ToListAsync(cancellationToken);

			return taskListByUserName;
		}

		private static List<GetTaskListForUserDto> MapTaskListByIdToDto(List<TaskList> taskLists)
		{
            return taskLists
				.Select(tl => new GetTaskListForUserDto
				{
					TaskListId = tl.TaskListId,
					TaskListName = tl.TaskListName,
					UserName = tl.UserName,
					TaskItems = tl.TaskItems.Select(ti => new GetTaskItemForUserDto
					{
						TaskItemId = ti.TaskItemId,
						TaskItemName = ti.TaskItemName
					}).ToList()
				}).ToList();
        }
	}
}
