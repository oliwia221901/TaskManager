﻿using FluentValidation;
using TaskManagerAPI.Application.Dtos.UsersManage.ChangePassword;

namespace TaskManagerAPI.Application.UsersManage.ChangePassword.Commands
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Old password is required.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
