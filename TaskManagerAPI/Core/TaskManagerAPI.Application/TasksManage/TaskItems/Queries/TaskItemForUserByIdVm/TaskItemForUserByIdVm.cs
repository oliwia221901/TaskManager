using TaskManagerAPI.Application.Dtos.GetTaskItemForUserById;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Queries
{
    public class TaskItemForUserByIdVm
    {
        public IEnumerable<GetTaskListForUserByIdDto> TaskLists { get; set; }
    }
}
