using FluentValidation;
using TaskManagerAPI.Application.Dtos.ResetPassword;

namespace TaskManagerAPI.Application.UsersManage.ResetPassword
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Old password is required.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}

