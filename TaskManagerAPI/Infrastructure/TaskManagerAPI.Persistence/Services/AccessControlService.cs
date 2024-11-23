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

        public async Task EnsureUserHasAccess(string userId, int taskItemId, PermissionLevel requiredLevel, CancellationToken cancellationToken)
        {
            if (await IsUserTaskListCreator(userId, taskItemId, cancellationToken))
                return;

            await CheckTaskItemPermissions(userId, taskItemId, requiredLevel, cancellationToken);
        }


        private async Task<bool> IsUserTaskListCreator(string userId, int taskItemId, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.TaskItems
                .Include(tl => tl.TaskLists)
                .Where(ti => ti.TaskItemId == taskItemId)
                .Select(ti => ti.TaskLists.UserId)
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
                throw new ForbiddenAccessException($"You do not have the required {requiredLevel} permission for this TaskItem.");
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
