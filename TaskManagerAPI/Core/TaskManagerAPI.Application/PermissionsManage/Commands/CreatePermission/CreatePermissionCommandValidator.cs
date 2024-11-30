using FluentValidation;
using TaskManagerAPI.Application.PermissionsManage.Commands.CreatePermission;

namespace TaskManagerAPI.Application.PermissionsManage.Commands
{
    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
	{
		public CreatePermissionCommandValidator()
		{
            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId cannot be null.")
                .NotEmpty().WithMessage("UserId cannot be empty.");

            RuleFor(x => x.CreatePermissionDto.TaskListId)
                .GreaterThan(0).WithMessage("TaskListId must be grater than 0.");

            RuleFor(x => x.CreatePermissionDto.TaskItemId)
                .GreaterThan(0).WithMessage("TaskItemId must be grater than 0.");

            RuleFor(x => x.CreatePermissionDto.Level)
                .NotNull().WithMessage("Level is required.")
                .IsInEnum().WithMessage("Level must be between 1 and 3.");
        }
	}
}

