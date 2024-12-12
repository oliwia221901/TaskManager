using MediatR;
using TaskManagerAPI.Application.Dtos.TasksManage.UpdateTask;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommand : IRequest
    {
        public int TaskItemId { get; set; }
        public UpdateTaskItemDto UpdateTaskItemDto { get; set; }
    }
}
