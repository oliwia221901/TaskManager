using MediatR;

namespace TaskManagerAPI.Application.Features.Register
{
    public class RegisterUserCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

