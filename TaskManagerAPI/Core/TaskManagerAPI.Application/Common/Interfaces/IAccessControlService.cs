using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface IAccessControlService
	{
        Task CheckRightsByTaskItem(string userId, int taskItemId, PermissionLevel requiredLevel, CancellationToken cancellationToken);
        Task CheckRightsByTaskList(string userId, int taskListId, PermissionLevel requiredLevel, CancellationToken cancellationToken);
    }
}
