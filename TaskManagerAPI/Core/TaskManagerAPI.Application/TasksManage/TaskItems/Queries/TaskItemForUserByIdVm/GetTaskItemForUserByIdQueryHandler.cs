using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.GetTaskItemForUserById;
using TaskManagerAPI.Domain.Entities.TaskManage;
using TaskManagerAPI.Application.TasksManage.TaskItems.Queries;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.TaskItems.Queries
{
    public class GetTaskItemForUserByIdQueryHandler : IRequestHandler<GetTaskItemForUserByIdQuery, TaskItemForUserByIdVm>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccessControlService _accessControlService;

        public GetTaskItemForUserByIdQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService, IAccessControlService accessControlService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _currentUserService = currentUserService;
            _accessControlService = accessControlService;
        }

        public async Task<TaskItemForUserByIdVm> Handle(GetTaskItemForUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            var taskLists = await GetTaskListByUserName(request.TaskItemId, cancellationToken);

            var userId = await GetUserId(userName, cancellationToken);

            await _accessControlService.CheckRightsByTaskItem(userId, request.TaskItemId, PermissionLevel.ReadOnly, cancellationToken);

            var taskListCreatorId = await GetTaskListCreatorId(request, cancellationToken);

            var creatorName = await GetTaskListCreatorName(taskListCreatorId, cancellationToken);

            var taskListsDto = MapTaskListsToDto(taskLists, request.TaskItemId, creatorName);

            return new TaskItemForUserByIdVm { TaskLists = taskListsDto };
        }

        private async Task<string> GetUserId(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("UserId was not found.");
        }

        private async Task<List<TaskList>> GetTaskListByUserName(int taskItemId, CancellationToken cancellationToken)
        {
            var taskItemExists = await _taskManagerDbContext.TaskItems
                .AnyAsync(ti => ti.TaskItemId == taskItemId, cancellationToken);

            if (!taskItemExists)
                throw new NotFoundException($"TaskItemId {taskItemId} was not found.");

            var taskLists = await _taskManagerDbContext.TaskLists
                .Where(tl => tl.TaskItems.Any(ti => ti.TaskItemId == taskItemId))
                .Include(tl => tl.TaskItems)
                .ToListAsync(cancellationToken);

            return taskLists;
        }

        private async Task<string> GetTaskListCreatorId(GetTaskItemForUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.TaskItems
                .Include(x => x.TaskLists)
                .Where(x => x.TaskItemId == request.TaskItemId)
                .Select(x => x.TaskLists.UserId)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("CreatorId was not found.");
        }

        private async Task<string> GetTaskListCreatorName(string taskListCreatorId, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(x => x.Id == taskListCreatorId)
                .Select(x => x.UserName)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("CreatorName was not found.");
        }

        private static List<GetTaskListForUserByIdDto> MapTaskListsToDto(List<TaskList> taskLists, int taskItemId, string creatorName)
        {
            return taskLists
                .Where(tl => tl.TaskItems.Any())
                .Select(tk => new GetTaskListForUserByIdDto
                {
                    TaskListId = tk.TaskListId,
                    TaskListName = tk.TaskListName,
                    UserName = creatorName,
                    TaskItems = tk.TaskItems
                        .Where(ti => ti.TaskItemId == taskItemId)
                        .Select(ti => new GetTaskItemForUserByIdDto
                        {
                            TaskItemId = ti.TaskItemId,
                            TaskItemName = ti.TaskItemName,
                            CreatedByUser = ti.CreatedByUser,
                            CreatedAt = ti.CreatedAt,
                            LastModifiedByUser = ti.LastModifiedByUser,
                            LastModifiedAt = ti.LastModifiedAt
                        }).ToList()
                }).ToList();
        }
    }
}
