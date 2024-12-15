using FluentValidation;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Commands.UpdateTaskList
{
	public class UpdateTaskListCommandValidator : AbstractValidator<UpdateTaskListCommand>
	{
		public UpdateTaskListCommandValidator()
		{
			RuleFor(x => x.TaskListId)
				.GreaterThan(0).WithMessage("TaskListId must be grater than 0.");
        }
	}
}
