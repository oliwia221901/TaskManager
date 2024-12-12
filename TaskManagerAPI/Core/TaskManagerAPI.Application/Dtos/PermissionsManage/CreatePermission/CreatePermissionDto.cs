using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.Dtos.PermissionsManage.CreatePermission
{
    public class CreatePermissionDto
	{
        public int TaskListId { get; set; }
        public int? TaskItemId { get; set; }
        public PermissionLevel Level { get; set; }
    }
}

