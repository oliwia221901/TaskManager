using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface IAccessControlService
	{
        Task CheckAccess(string userId, int itemId, PermissionLevel requiredLevel, bool isTaskList, CancellationToken cancellationToken);
        Task<List<TaskList>> GetAccessibleTaskLists(string currentUserId, CancellationToken cancellationToken);
    }
}
