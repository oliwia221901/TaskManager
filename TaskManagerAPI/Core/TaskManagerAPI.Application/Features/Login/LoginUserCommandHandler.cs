using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Features.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponse>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public LoginUserCommandHandler(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await FindUserAsync(request.Username);
            await ValidateUserAsync(user, request.Password);

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = CreateClaims(user, userRoles);

            var expirationTime = CalculateExpirationTime();
            var token = GenerateToken(authClaims, expirationTime);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expirationTime
            };
        }

        private async Task<IdentityUser> FindUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }
            return user;
        }

        private async Task ValidateUserAsync(IdentityUser user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }
        }

        private List<Claim> CreateClaims(IdentityUser user, IList<string> userRoles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return authClaims;
        }

        private DateTime CalculateExpirationTime()
        {
            var expiryMinutes = double.Parse(_configuration["Jwt:ExpiresInMinutes"]);
            return DateTime.Now.AddMinutes(expiryMinutes);
        }

        private JwtSecurityToken GenerateToken(IEnumerable<Claim> authClaims, DateTime expirationTime)
        {
            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                expires: expirationTime,
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256));
        }
    }
}

