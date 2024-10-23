using MediatR;

namespace TaskManagerAPI.Application.Features.Role.AssignRole
{
    public class AssignRoleCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}

