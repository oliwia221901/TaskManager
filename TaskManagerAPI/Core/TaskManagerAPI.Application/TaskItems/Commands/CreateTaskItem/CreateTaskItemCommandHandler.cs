using MediatR;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.CreateTask;
using TaskManagerAPI.Domain.Entities.TaskItem;

namespace TaskManagerAPI.Application.TaskItems.Commands
{
    public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, int>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;

        public CreateTaskItemCommandHandler(ITaskManagerDbContext taskManagerDbContext)
		{
			_taskManagerDbContext = taskManagerDbContext;
		}

		public async Task<int> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
		{
            var taskItem = CreateTaskItem(request.CreateTaskItemDto);

			_taskManagerDbContext.TaskItems.Add(taskItem);
			await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

			return taskItem.TaskItemId;
		}

		public static TaskItem CreateTaskItem(CreateTaskItemDto createTaskItemDto)
		{
			return new TaskItem
			{
				TaskItemName = createTaskItemDto.TaskItemName,
				TaskListId = createTaskItemDto.TaskListId
			};
		}
	}
}
