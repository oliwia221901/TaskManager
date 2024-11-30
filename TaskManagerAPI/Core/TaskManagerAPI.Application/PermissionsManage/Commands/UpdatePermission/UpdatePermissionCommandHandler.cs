using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.PermissionManage;

namespace TaskManagerAPI.Application.PermissionsManage.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public UpdatePermissionCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

        public async Task<Unit> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();
            var userId = await GetUserId(userName, cancellationToken);

            var permission = await GetPermission(request, cancellationToken);

            UpdatePermission(request, permission);

            _taskManagerDbContext.Permissions.Update(permission);
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<string> GetUserId(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(u => u.UserName == userName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("UserId was not found.");
        }

        private async Task<Permission> GetPermission(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.Permissions
                .SingleOrDefaultAsync(ti => ti.PermissionId == request.PermissionId, cancellationToken)
                ?? throw new NotFoundException($"PermissionId {request.PermissionId} was not found.");
        }

        private static void UpdatePermission(UpdatePermissionCommand request, Permission permission)
        {
            permission.Level = request.UpdatePermissionDto.Level;
        }
    }
}

