using TaskManagerAPI.Application.Dtos.UsersManage.GetAllUsers;

namespace TaskManagerAPI.Application.UsersManage.Users.Queries.GetAllUsers
{
    public class AllUsersVm
	{
        public IEnumerable<GetAllUsersDto> GetAllUsers { get; set; }
    }
}
