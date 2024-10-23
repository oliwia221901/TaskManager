using MediatR;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Features.Login
{
    public class LoginUserCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

