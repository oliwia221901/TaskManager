using TaskManagerAPI.Application.Dtos.TasksManage.GetTaskItemById;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Queries
{
    public class TaskItemByIdVm
    {
        public IEnumerable<GetTaskListByIdDto> TaskLists { get; set; }
    }
}
