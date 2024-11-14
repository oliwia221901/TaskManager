using FluentValidation;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
	{
		public CreateTaskItemCommandValidator()
		{
			RuleFor(x => x.CreateTaskItemDto.TaskItemName)
				.NotEmpty().WithMessage("TaskItemName is required.")
				.Length(3, 40).WithMessage("TaskItemName must be between 3 and 40 characters");

			RuleFor(x => x.CreateTaskItemDto.TaskListId)
				.GreaterThan(0).WithMessage("TaskListId must be greater than 0.");
		}
	}
}
