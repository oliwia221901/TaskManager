using MediatR;
using TaskManagerAPI.Application.Dtos.PermissionsManage.UpdatePermission;

namespace TaskManagerAPI.Application.PermissionsManage.Commands.UpdatePermission
{
    public class UpdatePermissionCommand : IRequest
	{
		public int PermissionId { get; set; }
		public UpdatePermissionDto UpdatePermissionDto { get; set; }
	}
}
