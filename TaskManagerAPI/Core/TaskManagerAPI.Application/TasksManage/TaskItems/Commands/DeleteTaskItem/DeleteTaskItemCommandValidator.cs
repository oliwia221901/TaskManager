using FluentValidation;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommandValidator : AbstractValidator<DeleteTaskItemCommand>
	{
		public DeleteTaskItemCommandValidator()
		{
			RuleFor(x => x.TaskItemId)
				.GreaterThan(0).WithMessage("TaskItemId must be grater than 0.");
		}
	}
}

