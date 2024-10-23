using MediatR;

namespace TaskManagerAPI.Application.Features.Role.AddRole
{
    public class AddRoleCommand : IRequest<string>
    {
        public string Role { get; set; }
    }
}

