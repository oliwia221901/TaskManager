using FluentValidation;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Commands.DeleteTaskList
{
    public class DeleteTaskListCommandValidator : AbstractValidator<DeleteTaskListCommand>
    {
        public DeleteTaskListCommandValidator()
        {
            RuleFor(x => x.TaskListId)
                .GreaterThan(0).WithMessage("TaskListId must be grater than 0.");
        }
    }
}

