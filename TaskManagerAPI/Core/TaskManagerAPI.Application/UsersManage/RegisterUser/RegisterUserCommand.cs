using MediatR;
using TaskManagerAPI.Application.Dtos.UsersManage.RegisterUser;

namespace TaskManagerAPI.Application.UsersManage.RegisterUser
{
    public class RegisterUserCommand : IRequest<string>
	{
        public RegisterUserDto RegisterUserDto { get; set; }
    }
}
