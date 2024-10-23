using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskManagerAPI.Application.Features.Role.AddRole
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, string>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<string> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(request.Role));
                if (result.Succeeded)
                {
                    return "Role added successfully";
                }

                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            throw new Exception("Role already exists");
        }
    }
}

