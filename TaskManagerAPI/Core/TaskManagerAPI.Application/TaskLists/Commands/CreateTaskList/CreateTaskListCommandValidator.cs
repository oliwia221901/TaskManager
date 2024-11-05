using FluentValidation;

namespace TaskManagerAPI.Application.TaskLists.Commands.CreateTaskList
{
    public class CreateTaskListCommandValidator : AbstractValidator<CreateTaskListCommand>
	{
		public CreateTaskListCommandValidator()
		{
			RuleFor(x => x.CreateTaskListDto.TaskListName)
				.NotEmpty().WithMessage("TaskListName is required.")
				.Length(3, 15).WithMessage("TaskListName must be between 3 and 15 characters.");
		}
	}
}
