using MediatR;
using TaskManagerAPI.Application.Dtos.UpdateTask;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommand : IRequest
    {
        public int TaskItemId { get; set; }
        public UpdateTaskItemDto UpdateTaskItemDto { get; set; }
    }
}

