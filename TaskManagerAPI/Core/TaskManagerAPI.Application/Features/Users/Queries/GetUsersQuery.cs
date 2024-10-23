using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskManagerAPI.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<List<IdentityUser>>
    {
    }
}

