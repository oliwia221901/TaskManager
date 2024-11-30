using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly ITaskManagerDbContext _dbContext;

        public AccessControlService(ITaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TaskList>> GetAccessibleTaskLists(string currentUserId, CancellationToken cancellationToken)
        {
            var ownedTaskLists = await _dbContext.TaskLists
                .Where(tl => tl.UserId == currentUserId)
                .Include(tl => tl.TaskItems)
                .ToListAsync(cancellationToken);

            var userPermissions = await _dbContext.Permissions
                .Where(p => p.UserId == currentUserId)
                .ToListAsync(cancellationToken);

            var permittedTaskListIds = userPermissions
                .Where(p => IsSufficientPermissionLevel(p.Level, PermissionLevel.ReadOnly))
                .Select(p => p.TaskListId)
                .Distinct()
                .ToList();

            var accessibleTaskLists = await _dbContext.TaskLists
                .Where(tl => permittedTaskListIds.Contains(tl.TaskListId))
                .Include(tl => tl.TaskItems)
                .ToListAsync(cancellationToken);

            foreach (var taskList in accessibleTaskLists)
            {
                taskList.TaskItems = taskList.TaskItems
                    .Where(ti =>
                        userPermissions.Any(p =>
                            (p.TaskItemId == ti.TaskItemId && IsSufficientPermissionLevel(p.Level, PermissionLevel.ReadOnly)) ||
                            (p.TaskListId == ti.TaskListId && p.TaskItemId == null && IsSufficientPermissionLevel(p.Level, PermissionLevel.ReadOnly))
                        ))
                    .ToList();
            }

            return ownedTaskLists.Union(accessibleTaskLists).ToList();
        }

        public async Task CheckAccess(string userId, int itemId, PermissionLevel requiredLevel, bool isTaskList, CancellationToken cancellationToken)
        {
            if (await IsCreator(userId, itemId, isTaskList, cancellationToken))
                return;

            if (!await DoesItemExist(itemId, isTaskList, cancellationToken))
                throw new NotFoundException($"{(isTaskList ? "TaskListId" : "TaskItemId")} {itemId} was not found.");

            if (!isTaskList)
            {
                var taskListId = await _dbContext.TaskItems
                    .Where(ti => ti.TaskItemId == itemId)
                    .Select(ti => ti.TaskListId)
                    .SingleOrDefaultAsync(cancellationToken);

                var taskPermission = await _dbContext.Permissions
                     .Where(p => p.TaskItemId == itemId && p.UserId == userId && p.TaskListId == taskListId)
                     .Select(p => p.Level)
                     .FirstOrDefaultAsync(cancellationToken);

                if (taskPermission != PermissionLevel.None)
                {
                    if (IsSufficientPermissionLevel(taskPermission, requiredLevel))
                        return;
                    else
                        throw new ForbiddenAccessException($"Unauthorized access. Required {requiredLevel} permission for this operation.");
                }

                var listPermission = await _dbContext.Permissions
                    .Where(p => p.TaskListId == taskListId && p.UserId == userId && p.TaskItemId == null)
                    .Select(p => p.Level)
                    .SingleOrDefaultAsync(cancellationToken);

                if (IsSufficientPermissionLevel(listPermission, requiredLevel))
                    return;

                throw new ForbiddenAccessException($"Unauthorized access. Required {requiredLevel} permission for this operation.");
            }
            else
            {
                var listPermission = await _dbContext.Permissions
                    .Where(p => p.TaskListId == itemId && p.UserId == userId && p.TaskItemId == null)
                    .Select(p => p.Level)
                    .SingleOrDefaultAsync(cancellationToken);

                if (listPermission == default || !IsSufficientPermissionLevel(listPermission, requiredLevel))
                {
                    throw new ForbiddenAccessException($"Unauthorized access. Required {requiredLevel} permission for this operation.");
                }
            }
        }

        private async Task<bool> DoesItemExist(int itemId, bool isTaskList, CancellationToken cancellationToken)
        {
            var exists = isTaskList
                ? await _dbContext.TaskLists
                    .AnyAsync(tl => tl.TaskListId == itemId, cancellationToken)
                : await _dbContext.TaskItems
                    .AnyAsync(ti => ti.TaskItemId == itemId, cancellationToken);

            return exists;
        }

        private async Task<bool> IsCreator(string userId, int itemId, bool isTaskList, CancellationToken cancellationToken)
        {
            var isCreator = isTaskList
                ? await _dbContext.TaskLists
                    .Where(tl => tl.TaskListId == itemId && tl.UserId == userId)
                    .AnyAsync(cancellationToken)
                : await _dbContext.TaskItems
                    .Where(ti => ti.TaskItemId == itemId)
                    .Select(ti => ti.TaskLists.UserId)
                    .AnyAsync(creatorId => creatorId == userId, cancellationToken);

            return isCreator;
        }

        private static bool IsSufficientPermissionLevel(PermissionLevel userLevel, PermissionLevel requiredLevel)
        {
            bool isSufficient = userLevel switch
            {
                PermissionLevel.FullControl => true,
                PermissionLevel.ReadWrite => requiredLevel is PermissionLevel.ReadWrite or PermissionLevel.ReadOnly,
                PermissionLevel.ReadOnly => requiredLevel == PermissionLevel.ReadOnly,
                _ => false
            };

            return isSufficient;
        }
    }
}
