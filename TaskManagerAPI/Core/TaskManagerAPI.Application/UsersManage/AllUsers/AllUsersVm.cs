using TaskManagerAPI.Application.Dtos.UsersManage.GetAllUsers;

namespace TaskManagerAPI.Application.UsersManage.AllUsers
{
    public class AllUsersVm
	{
        public IEnumerable<GetAllUsersDto> GetAllUsers { get; set; }
    }
}
