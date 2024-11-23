using MediatR;
using TaskManagerAPI.Application.Dtos.UpdateTask;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemCommand : IRequest
    {
        public UpdateTaskItemDto UpdateTaskItemDto { get; set; }
    }
}

