using MediatR;
using TaskManagerAPI.Application.Dtos.LoginUser;

namespace TaskManagerAPI.Application.UsersManage.LoginUser
{
    public class LoginUserCommand : IRequest<string>
    {
        public LoginUserDto LoginUserDto { get; set; }
    }
}
