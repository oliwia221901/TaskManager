using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.UsersManage.GetUsers;
using TaskManagerAPI.Domain.Entities.UserManage;
using TaskManagerAPI.Domain.Entities.UserManage.Enums;

namespace TaskManagerAPI.Application.UsersManage.Users
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, UsersVm>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;

		public GetUsersQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
		}

        public async Task<UsersVm> Handle(GetUsersQuery request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();

			var users = await GetUsers(userName, cancellationToken);
			var usersToDto = MapUsersToDto(users);

			return new UsersVm
			{
				GetUsers = usersToDto
			};
		}

        public async Task<List<AppUser>> GetUsers(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Include(u => u.FriendRequestsSent)
                .Include(u => u.FriendRequestsReceived)
                .Where(u => u.UserName != userName &&
                            !u.FriendRequestsSent.Any(f =>
                                (f.Requester.UserName == userName || f.Friend.UserName == userName) &&
                                (f.Status == FriendshipStatus.Pending || f.Status == FriendshipStatus.Accepted)) &&
                            !u.FriendRequestsReceived.Any(f =>
                                (f.Requester.UserName == userName || f.Friend.UserName == userName) &&
                                (f.Status == FriendshipStatus.Pending || f.Status == FriendshipStatus.Accepted)))
                .OrderBy(u => u.UserName)
                .ToListAsync(cancellationToken);
        }




        public static List<GetUsersDto> MapUsersToDto(List<AppUser> users)
		{
			return users
				.Select(u => new GetUsersDto
				{
					UserId = u.Id,
					UserName = u.UserName
				}).ToList();
		}
    }
}

