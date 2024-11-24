using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;

        public AccessControlService(ITaskManagerDbContext taskManagerDbContext)
        {
            _taskManagerDbContext = taskManagerDbContext;
        }

        public async Task CheckRightsByTaskItem(string userId, int taskItemId, PermissionLevel requiredLevel, CancellationToken cancellationToken)
        {
            if (await IsUserTaskListCreator(userId, taskItemId, cancellationToken))
                return;

            await CheckTaskItemPermissions(userId, taskItemId, requiredLevel, cancellationToken);
        }

        public async Task CheckRightsByTaskList(string userId, int taskListId, PermissionLevel requiredLevel, CancellationToken cancellationToken)
        {
            if (await IsUserTaskListCreatorCreate(userId, taskListId, cancellationToken))
                return;

            await CheckTaskItemPermissions(userId, taskListId, requiredLevel, cancellationToken);
        }

        private async Task<bool> IsUserTaskListCreator(string userId, int taskItemId, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.TaskItems
                .Include(tl => tl.TaskLists)
                .Where(ti => ti.TaskItemId == taskItemId)
                .Select(ti => ti.TaskLists.UserId)
                .AnyAsync(creatorId => creatorId == userId, cancellationToken);
        }

        private async Task<bool> IsUserTaskListCreatorCreate(string userId, int taskListId, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.TaskLists
                .Where(ti => ti.TaskListId == taskListId)
                .Select(ti => ti.UserId)
                .AnyAsync(creatorId => creatorId == userId, cancellationToken);
        }

        private async Task CheckTaskItemPermissions(string userId, int taskItemId, PermissionLevel requiredLevel, CancellationToken cancellationToken)
        {
            var permissions = await _taskManagerDbContext.Permissions
                .Where(p => p.TaskId == taskItemId && p.UserId == userId)
                .ToListAsync(cancellationToken);

            var hasPermission = permissions
                .Any(p => IsSufficientPermissionLevel(p.Level, requiredLevel));

            if (!hasPermission)
                throw new ForbiddenAccessException($"Unauthorized access. Required {requiredLevel} permission for this operation.");
        }

        private static bool IsSufficientPermissionLevel(PermissionLevel userLevel, PermissionLevel requiredLevel)
        {
            return userLevel switch
            {
                PermissionLevel.FullControl => true,
                PermissionLevel.ReadWrite => requiredLevel == PermissionLevel.ReadWrite || requiredLevel == PermissionLevel.ReadOnly,
                PermissionLevel.ReadOnly => requiredLevel == PermissionLevel.ReadOnly,
                _ => false
            };
        }
    }
}
