using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface IAccessControlService
	{
        Task EnsureUserHasAccess(string userId, int taskItemId, PermissionLevel requiredLevel, CancellationToken cancellationToken);
    }
}

