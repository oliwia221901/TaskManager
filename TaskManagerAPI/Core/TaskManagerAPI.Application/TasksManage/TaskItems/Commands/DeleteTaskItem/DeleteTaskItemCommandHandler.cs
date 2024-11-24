using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly IAccessControlService _accessControlService;

		public DeleteTaskItemCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService, IAccessControlService accessControlService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
			_accessControlService = accessControlService;
		}

		public async Task<Unit> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();

            var userId = await GetUserId(userName, cancellationToken);

			await _accessControlService.CheckRightsByTaskItem(userId, request.TaskItemId, PermissionLevel.FullControl, cancellationToken);

			var taskItem = await GetTaskItem(request.TaskItemId, cancellationToken);

			_taskManagerDbContext.TaskItems.Remove(taskItem);
			await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		private async Task<string> GetUserId(string userName, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.AppUsers
				.Where(x => x.UserName == userName)
				.Select(x => x.Id)
				.SingleOrDefaultAsync(cancellationToken)
				?? throw new NotFoundException("UserId was not found.");
		}

		private async Task<TaskItem> GetTaskItem(int taskItemId, CancellationToken cancellationToken)
		{
			return await _taskManagerDbContext.TaskItems
				.SingleOrDefaultAsync(x => x.TaskItemId == taskItemId, cancellationToken)
				?? throw new NotFoundException("TaskItemId was not found.");
		}
	}
}

