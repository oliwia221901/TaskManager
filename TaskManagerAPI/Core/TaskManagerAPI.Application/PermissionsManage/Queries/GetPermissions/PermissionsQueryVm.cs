using TaskManagerAPI.Application.Dtos.PermissionsManage.GetPermissions;

namespace TaskManagerAPI.Application.PermissionsManage.Queries.GetPermissions
{
	public class PermissionsQueryVm
	{
		public IEnumerable<GetPermissionsDto> GetPermissionsDtos { get; set; }
	}
}
