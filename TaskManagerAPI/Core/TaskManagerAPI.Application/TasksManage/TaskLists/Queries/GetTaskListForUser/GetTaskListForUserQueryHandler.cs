using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.GetTaskListForUser;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Queries.GetTaskListForUser
{
    public class GetTaskListForUserQueryHandler : IRequestHandler<GetTaskListForUserQuery, TaskListForUserVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;
        private readonly IAccessControlService _accessControlService;

		public GetTaskListForUserQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService, IAccessControlService accessControlService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
            _accessControlService = accessControlService;
		}

        public async Task<TaskListForUserVm> Handle(GetTaskListForUserQuery request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();
            var currentUserId = await GetCurrentUserId(userName, cancellationToken);

            var accessibleTaskLists = await _accessControlService.GetAccessibleTaskLists(currentUserId, cancellationToken);

            var taskListsDto = MapTaskListByIdToDto(accessibleTaskLists, userName);

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

        private static List<GetTaskListForUserDto> MapTaskListByIdToDto(List<TaskList> taskLists, string userName)
        {
            return taskLists
                .Select(tl => new GetTaskListForUserDto
                {
                    TaskListId = tl.TaskListId,
                    TaskListName = tl.TaskListName,
                    UserName = userName,
                    TaskItems = tl.TaskItems
                        .Select(ti => new GetTaskItemForUserDto
                        {
                            TaskItemId = ti.TaskItemId,
                            TaskItemName = ti.TaskItemName
                        })
                        .ToList()
                })
                .ToList();
        }

    }
}
