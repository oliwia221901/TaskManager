using TaskManagerAPI.Domain.Entities.UserManage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace TaskManagerAPI.Application.Common.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GenerateToken(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null");

            var userName = user.UserName ?? throw new InvalidOperationException("UserName cannot be null.");
            var email = user.Email ?? throw new InvalidOperationException("Email cannot be null.");

            var claims = CreateClaims(userName, email);

            var secretKey = _configuration["JwtSettings:SecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JWT Secret Key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = CreateJwtSecurityToken(creds, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static Claim[] CreateClaims(string userName, string email)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName)
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken(SigningCredentials creds, Claim[] claims)
        {
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("JWT Issuer or Audience is not configured.");

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryInMinutes"])),
                signingCredentials: creds
            );
        }

        public async Task<string> GenerateTokenAsync(AppUser user)
        {
            return await Task.FromResult(GenerateToken(user));
        }
    }
}
