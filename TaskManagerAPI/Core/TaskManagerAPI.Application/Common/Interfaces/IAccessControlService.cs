using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface IAccessControlService
	{
        Task CheckReadUpdateRights(string userId, int taskItemId, PermissionLevel requiredLevel, CancellationToken cancellationToken);
        Task CheckCreateDeleteRights(string userId, int taskListId, PermissionLevel requiredLevel, CancellationToken cancellationToken);
    }
}
