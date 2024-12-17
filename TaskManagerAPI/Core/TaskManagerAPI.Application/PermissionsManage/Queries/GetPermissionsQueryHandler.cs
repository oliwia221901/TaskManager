using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.PermissionsManage.GetPermissions;
using TaskManagerAPI.Domain.Entities.PermissionManage;

namespace TaskManagerAPI.Application.PermissionsManage.Queries
{
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, PermissionsQueryVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public GetPermissionsQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

		public async Task<PermissionsQueryVm> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();

			var userId = await GetUserId(userName, cancellationToken);

			var permissions = await GetPermissions(userId, cancellationToken);

			var permissionsToDto = MapPermissionsToDto(permissions);

			return new PermissionsQueryVm
			{
				GetPermissionsDtos = permissionsToDto
			};

		}

		private async Task<string> GetUserId(string userName, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.AppUsers
				.Where(x => x.UserName == userName)
				.Select(x => x.Id)
				.SingleOrDefaultAsync(cancellationToken)
				?? throw new NotFoundException("UserId was not found.");
		}

		private async Task<List<Permission>> GetPermissions(string userId, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.Permissions
				.Where(x => x.CreatedBy == userId)
				.ToListAsync(cancellationToken);
		}

		private static List<GetPermissionsDto> MapPermissionsToDto(List<Permission> permissions)
		{
			return permissions
				.Select(p => new GetPermissionsDto
				{
					PermissionId = p.PermissionId,
					UserId = p.UserId,
					TaskListId = p.TaskListId,
					TaskItemId = p.TaskItemId,
					Level = p.Level
				}).ToList();
		}
	}
}
