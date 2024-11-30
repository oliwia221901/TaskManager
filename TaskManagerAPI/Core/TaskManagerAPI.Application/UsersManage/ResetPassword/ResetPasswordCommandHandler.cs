using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.UserManage;

namespace TaskManagerAPI.Application.UsersManage.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public ResetPasswordHandler(UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();
            var user = await GetUserInfo(userName);

            await ValidateOldPassword(user, request.ResetPasswordDto.OldPassword);
            ValidateNewPassword(request.ResetPasswordDto.OldPassword, request.ResetPasswordDto.NewPassword);

            await ChangePasswordAsync(user, request.ResetPasswordDto.OldPassword, request.ResetPasswordDto.NewPassword);

            return Unit.Value;
        }

        private async Task<AppUser> GetUserInfo(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName)
                ?? throw new KeyNotFoundException("User was not found.");

            return user;
        }

        private async Task ValidateOldPassword(AppUser user, string oldPassword)
        {
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isPasswordCorrect)
            {
                throw new InvalidOperationException("Old password is incorrect.");
            }
        }

        private static void ValidateNewPassword(string oldPassword, string newPassword)
        {
            if (oldPassword == newPassword)
            {
                throw new InvalidOperationException("New password cannot be the same as the old password.");
            }
        }

        private async Task ChangePasswordAsync(AppUser user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to reset password. Errors: {errors}");
            }
        }
    }
}

