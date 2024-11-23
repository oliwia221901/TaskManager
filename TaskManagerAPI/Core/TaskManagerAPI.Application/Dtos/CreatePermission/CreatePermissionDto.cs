using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.Dtos.CreatePermission
{
    public class CreatePermissionDto
	{
        public int TaskListId { get; set; }
        public int? TaskId { get; set; }
        public PermissionLevel Level { get; set; }
    }
}

