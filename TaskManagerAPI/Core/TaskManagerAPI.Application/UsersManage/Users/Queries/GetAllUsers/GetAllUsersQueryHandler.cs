using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.UsersManage.GetAllUsers;
using TaskManagerAPI.Application.UsersManage.Users.Queries.GetAllUsers;
using TaskManagerAPI.Domain.Entities.UserManage;

namespace TaskManagerAPI.Application.UsersManage.Users
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, AllUsersVm>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetAllUsersQueryHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<AllUsersVm> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            var users = await GetAllUsers(userName, cancellationToken);
            var usersToDto = MapUsersToDto(users);

            return new AllUsersVm
            {
                GetAllUsers = usersToDto
            };
        }

        public async Task<List<AppUser>> GetAllUsers(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(u => u.UserName != userName)
                .OrderBy(u => u.UserName)
                .ToListAsync(cancellationToken);
        }

        public static List<GetAllUsersDto> MapUsersToDto(List<AppUser> users)
        {
            return users
                .Select(u => new GetAllUsersDto
                {
                    UserId = u.Id,
                    UserName = u.UserName
                }).ToList();
        }
    }
}
