using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Domain.Entities.UserManage;

namespace TaskManagerAPI.Application.UsersManage.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                UserName = request.RegisterUserDto.Username,
                Email = request.RegisterUserDto.Email
            };

            var result = await _userManager.CreateAsync(user, request.RegisterUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new System.Exception($"User registration failed: {errors}");
            }

            return "User registered successfully";
        }
    }
}
