using FluentValidation;
using TaskManagerAPI.Application.TasksManage.TaskItems.Commands.UpdateTaskItem;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Validators
{
    public class UpdateTaskItemCommandValidator : AbstractValidator<UpdateTaskItemCommand>
    {
        public UpdateTaskItemCommandValidator()
        {
            RuleFor(x => x.TaskItemId)
                .GreaterThan(0).WithMessage("TaskItemId must be greater than 0.");

            When(x => x.UpdateTaskItemDto != null, () =>
            {
                RuleFor(x => x.UpdateTaskItemDto.TaskItemName)
                .NotEmpty().WithMessage("TaskItemName cannot be empty.")
                .MaximumLength(100).WithMessage("TaskItemName cannot exceed 100 characters.");
            });
        }
    }
}
