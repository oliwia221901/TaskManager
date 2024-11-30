using FluentValidation;

namespace TaskManagerAPI.Application.PermissionsManage.Commands.UpdatePermission
{
    public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionCommandValidator()
        {
            RuleFor(x => x.PermissionId)
                .GreaterThan(0).WithMessage("TaskListId must be grater than 0.");

            RuleFor(x => x.UpdatePermissionDto.Level)
                .NotNull().WithMessage("Level is required.")
                .IsInEnum().WithMessage("Level must be between 1 and 3.");
        }
    }
}