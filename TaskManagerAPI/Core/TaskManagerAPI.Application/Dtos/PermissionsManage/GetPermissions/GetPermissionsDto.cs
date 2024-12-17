using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.Dtos.PermissionsManage.GetPermissions
{
    public class GetPermissionsDto
	{
        public int PermissionId { get; set; }
        public string UserId { get; set; }
        public int TaskListId { get; set; }
        public int? TaskItemId { get; set; }
        public PermissionLevel Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
