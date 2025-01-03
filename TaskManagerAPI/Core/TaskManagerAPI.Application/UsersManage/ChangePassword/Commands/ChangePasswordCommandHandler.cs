using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.Email;
using TaskManagerAPI.Application.UsersManage.ChangePassword.Commands;
using TaskManagerAPI.Domain.Entities.UserManage;

namespace TaskManagerAPI.Application.UsersManage.Commands.ChangePassword
{
    public class ResetPasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailService _emailService;

        public ResetPasswordHandler(UserManager<AppUser> userManager, ICurrentUserService currentUserService, IEmailService emailService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();
            var user = await GetUserInfo(userName);

            await ValidateOldPassword(user, request.ChangePasswordDto.OldPassword);
            ValidateNewPassword(request.ChangePasswordDto.OldPassword, request.ChangePasswordDto.NewPassword);

            await ChangePasswordAsync(user, request.ChangePasswordDto.OldPassword, request.ChangePasswordDto.NewPassword);

            await SendPasswordChangedEmail(user.Email);

            return Unit.Value;
        }

        private async Task<AppUser> GetUserInfo(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName)
                ?? throw new NotFoundException("User was not found.");

            return user;
        }

        private async Task ValidateOldPassword(AppUser user, string oldPassword)
        {
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isPasswordCorrect)
            {
                throw new BadRequestException("Old password is incorrect.");
            }
        }

        private static void ValidateNewPassword(string oldPassword, string newPassword)
        {
            if (oldPassword == newPassword)
            {
                throw new BadRequestException("New password cannot be the same as the old password.");
            }
        }

        private async Task ChangePasswordAsync(AppUser user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Failed to reset password. Errors: {errors}");
            }
        }

        private async Task SendPasswordChangedEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new BadRequestException("Email cannot be null or empty.");
            }

            var emailDto = new EmailDto
            {
                To = email,
                Subject = "Change Password Confirmation",
                Body = "Your password has been successfully changed. If you did not request this change, please contact support immediately."
            };

            await Task.Run(() => _emailService.SendEmail(emailDto));
        }

    }
}

