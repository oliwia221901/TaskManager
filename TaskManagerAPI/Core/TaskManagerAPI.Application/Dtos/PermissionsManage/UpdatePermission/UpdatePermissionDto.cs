using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.Dtos.PermissionsManage.UpdatePermission
{
    public class UpdatePermissionDto
	{
        public PermissionLevel Level { get; set; }
    }
}
