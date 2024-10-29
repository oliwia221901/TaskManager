using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskManagerAPI.Application.Common.Interfaces;

namespace TaskManagerAPI.Persistence.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserName()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            var claimsIdentity = user.Identity as ClaimsIdentity
                ?? throw new InvalidOperationException("ClaimsIdentity is null.");

            var userNameClaim = claimsIdentity.FindFirst(ClaimTypes.Name)
                ?? throw new UnauthorizedAccessException("UserNameClaim is not found.");

            return userNameClaim.Value;
        }
    }
}
