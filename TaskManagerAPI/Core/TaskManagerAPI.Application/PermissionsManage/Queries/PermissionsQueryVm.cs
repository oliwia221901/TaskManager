using TaskManagerAPI.Application.Dtos.PermissionsManage.GetPermissions;

namespace TaskManagerAPI.Application.PermissionsManage.Queries
{
	public class PermissionsQueryVm
	{
		public IEnumerable<GetPermissionsDto> GetPermissionsDtos { get; set; }
	}
}
