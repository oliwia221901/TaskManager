using MediatR;
using TaskManagerAPI.Application.Dtos.RegisterUser;

namespace TaskManagerAPI.Application.UsersManage.RegisterUser
{
    public class RegisterUserCommand : IRequest<string>
	{
        public RegisterUserDto RegisterUserDto { get; set; }
    }
}
