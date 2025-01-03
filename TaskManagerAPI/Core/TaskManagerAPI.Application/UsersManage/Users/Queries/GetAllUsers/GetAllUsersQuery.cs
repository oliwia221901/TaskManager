using MediatR;
using TaskManagerAPI.Application.UsersManage.Users.Queries.GetAllUsers;

namespace TaskManagerAPI.Application.UsersManage.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<AllUsersVm>
	{
	}
}

