using FluentValidation;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Queries.TaskItemById
{
    public class GetTaskItemByIdQueryValidator : AbstractValidator<GetTaskItemByIdQuery>
    {
        public GetTaskItemByIdQueryValidator()
        {
            RuleFor(x => x.TaskItemId)
                .GreaterThan(0).WithMessage("TaskItemId must be greater than 0.");
        }
    }
}
