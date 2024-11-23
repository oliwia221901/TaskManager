using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
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
			var currentUserId = await GetCurrentUserId(userName, cancellationToken);
			var taskLists = await GetTaskListByUserId(currentUserId, cancellationToken);
			var taskListsDto = MapTaskListByIdToDto(taskLists, userName);

            return new TaskListForUserVm
			{
				TaskLists = taskListsDto
			};
        }

		private async Task<string> GetCurrentUserId(string userName, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.AppUsers
				.Where(x => x.UserName == userName)
				.Select(x => x.Id)
				.SingleOrDefaultAsync(cancellationToken)
				?? throw new NotFoundException("CurrentUserId was not found.");
		}

		private async Task<List<TaskList>> GetTaskListByUserId(string currentUserId, CancellationToken cancellationToken)
		{
			var taskListByUserId = await _taskManagerDbContext.TaskLists
				.Where(tl => tl.UserId == currentUserId)
				.Include(tl => tl.TaskItems)
				.ToListAsync(cancellationToken);

			return taskListByUserId;
		}

		private static List<GetTaskListForUserDto> MapTaskListByIdToDto(List<TaskList> taskLists, string userName)
		{
            return taskLists
				.Select(tl => new GetTaskListForUserDto
				{
					TaskListId = tl.TaskListId,
					TaskListName = tl.TaskListName,
					UserName = userName,
					TaskItems = tl.TaskItems.Select(ti => new GetTaskItemForUserDto
					{
						TaskItemId = ti.TaskItemId,
						TaskItemName = ti.TaskItemName
					}).ToList()
				}).ToList();
        }
	}
}
