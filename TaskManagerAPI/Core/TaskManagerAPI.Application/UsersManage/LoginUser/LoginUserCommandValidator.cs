using FluentValidation;

namespace TaskManagerAPI.Application.UsersManage.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
	{
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.LoginUserDto.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.LoginUserDto.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
