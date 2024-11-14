using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.GetUsers;
using TaskManagerAPI.Domain.Entities.UserManage;

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
                .Where(x => x.UserName != userName)
				.OrderBy(x => x.UserName)
                .ToListAsync(cancellationToken);
        }

        public static List<GetUsersDto> MapUsersToDto(List<AppUser> users)
		{
			return users
				.Select(u => new GetUsersDto
				{
					UserName = u.UserName
				}).ToList();
		}
    }
}

