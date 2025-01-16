using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.GetTaskList;
using TaskManagerAPI.Application.Dtos.TasksManage.GetTaskList;
using TaskManagerAPI.Application.TasksManage.TaskLists.Queries.GetTaskList;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Queries.GetTaskListForUser
{
    public class GetTaskListQueryHandler : IRequestHandler<GetTaskListQuery, TaskListVm>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccessControlService _accessControlService;

        public GetTaskListQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService, IAccessControlService accessControlService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _currentUserService = currentUserService;
            _accessControlService = accessControlService;
        }

        public async Task<TaskListVm> Handle(GetTaskListQuery request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();
            var currentUserId = await GetCurrentUserId(userName, cancellationToken);

            var accessibleTaskLists = await _accessControlService.GetAccessibleTaskLists(currentUserId, cancellationToken);

            var taskListsDto = await MapTaskListByIdToDto(accessibleTaskLists, cancellationToken);

            return new TaskListVm
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
                ?? throw new NotFoundException("UserId was not found.");
        }

        private async Task<string> GetUserNameById(string userId, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(x => x.Id == userId)
                .Select(x => x.UserName)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("UserName was not found.");
        }

        private async Task<List<GetTaskListDto>> MapTaskListByIdToDto(List<TaskList> taskLists, CancellationToken cancellationToken)
        {
            var taskListsDto = new List<GetTaskListDto>();

            foreach (var tl in taskLists)
            {
                var ownerUserName = await GetUserNameById(tl.UserId, cancellationToken);

                taskListsDto.Add(new GetTaskListDto
                {
                    TaskListId = tl.TaskListId,
                    TaskListName = tl.TaskListName,
                    UserName = ownerUserName,
                    TaskItems = tl.TaskItems
                        .Select(ti => new GetTaskItemDto
                        {
                            TaskItemId = ti.TaskItemId,
                            TaskItemName = ti.TaskItemName
                        })
                        .ToList()
                });
            }

            return taskListsDto;
        }
    }
}
