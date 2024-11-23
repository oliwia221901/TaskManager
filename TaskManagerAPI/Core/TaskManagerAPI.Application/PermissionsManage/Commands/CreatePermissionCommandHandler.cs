using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.PermissionManage;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.PermissionsManage.Commands
{
    public class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand, int>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly ICurrentUserService _currentUserService;

        public CreatePermissionHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            await CheckIfPermissionExists(request, cancellationToken);

            var userId = await GetCurrentUserId(userName, cancellationToken);

            await CheckIfFriendshipExists(userId, request, cancellationToken);

            var permission = CreatePermission(request);

            _taskManagerDbContext.Permissions.Add(permission);
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return permission.PermissionId;
        }

        public async Task<bool> CheckIfPermissionExists(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var exists = await _taskManagerDbContext.Permissions
                .AnyAsync(p =>
                p.UserId == request.UserId &&
                p.TaskListId == request.CreatePermissionDto.TaskListId &&
                p.TaskId == request.CreatePermissionDto.TaskId &&
                p.Level == request.CreatePermissionDto.Level,
                cancellationToken);

            if (exists)
                throw new ResourceConflictException("This permission already exists.");

            return exists;
        }

        public async Task<string> GetCurrentUserId(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("User was not found.");
        }

        public async Task<bool> CheckIfFriendshipExists(string userId, CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var exists = await _taskManagerDbContext.Friendships
                .AnyAsync(x => ((x.RequesterId == userId && x.FriendId == request.UserId) ||
                                (x.RequesterId == request.UserId && x.FriendId == userId)) &&
                                x.Status == FriendshipStatus.Accepted, cancellationToken);

            if (!exists)
                throw new NotFoundException("Friendship was not found.");

            return exists;
        }

        public static Permission CreatePermission(CreatePermissionCommand request)
        {
            return new Permission
            {
                UserId = request.UserId,
                TaskListId = request.CreatePermissionDto.TaskListId,
                TaskId = request.CreatePermissionDto.TaskId,
                Level = request.CreatePermissionDto.Level
            };
        }

    }
}

