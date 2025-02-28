﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Application.Common.Services;
using TaskManagerAPI.Application.UsersManage.LoginUser.Commands;
using TaskManagerAPI.Domain.Entities.UserManage;

namespace TaskManagerAPI.Application.UsersManage.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        public LoginUserCommandHandler(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.LoginUserDto.UserName)
                ?? throw new UnauthorizedAccessException("Invalid username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginUserDto.Password, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid username or password");

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
