using MediatR;
using TaskManagerAPI.Application.Dtos.UsersManage.RegisterUser;

namespace TaskManagerAPI.Application.UsersManage.RegisterUser.Commands
{
    public class RegisterUserCommand : IRequest<string>
	{
        public RegisterUserDto RegisterUserDto { get; set; }
    }
}
