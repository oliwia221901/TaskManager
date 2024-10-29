using TaskManagerAPI.Application.Dtos.GetTaskItemForUserById;

namespace TaskManagerAPI.Application.TaskItems.Queries
{
    public class TaskItemForUserByIdVm
    {
        public IEnumerable<GetTaskListForUserByIdDto> TaskLists { get; set; }
    }
}
