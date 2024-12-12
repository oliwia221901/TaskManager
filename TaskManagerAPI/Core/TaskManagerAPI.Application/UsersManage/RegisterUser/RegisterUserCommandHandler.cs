using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Dtos.UsersManage.RegisterUser;
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
            if (await _userManager.FindByNameAsync(request.RegisterUserDto.Username) != null)
                throw new ResourceConflictException("Username already exists.");

            if (await _userManager.FindByEmailAsync(request.RegisterUserDto.Email) != null)
                throw new ResourceConflictException("Email already exists.");

            var user = RegisterAppUser(request.RegisterUserDto);

            await _userManager.CreateAsync(user, request.RegisterUserDto.Password);

            return user.Id;
        }

        public static AppUser RegisterAppUser(RegisterUserDto registerUserDto)
        {
            return new AppUser
            {
                UserName = registerUserDto.Username,
                Email = registerUserDto.Email
            };
        }
    }
}
