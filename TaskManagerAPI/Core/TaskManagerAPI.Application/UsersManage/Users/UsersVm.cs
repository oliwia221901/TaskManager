using TaskManagerAPI.Application.Dtos.GetUsers;

namespace TaskManagerAPI.Application.UsersManage.Users
{
    public class UsersVm
	{
		public IEnumerable<GetUsersDto> GetUsers { get; set; }
	}
}

