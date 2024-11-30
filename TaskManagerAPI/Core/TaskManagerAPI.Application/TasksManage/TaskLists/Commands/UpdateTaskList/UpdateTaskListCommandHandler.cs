using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Commands.UpdateTaskList
{
    public class UpdateTaskListCommandHandler : IRequestHandler<UpdateTaskListCommand>
	{
		private readonly ITaskManagerDbContext _taskManagerDbContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly IAccessControlService _accessControlService;

		public UpdateTaskListCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService, IAccessControlService accessControlService)
		{
			_taskManagerDbContext = taskManagerDbContext;
			_currentUserService = currentUserService;
			_accessControlService = accessControlService;
		}

		public async Task<Unit> Handle(UpdateTaskListCommand request, CancellationToken cancellationToken)
		{
			var userName = _currentUserService.GetCurrentUserName();
            var userId = await GetUserId(userName, cancellationToken);

            await _accessControlService.CheckAccess(userId, request.TaskListId, PermissionLevel.ReadWrite, true, cancellationToken);

            var taskList = await GetTaskList(request, cancellationToken);

            UpdateTaskList(request, taskList);

            _taskManagerDbContext.TaskLists.Update(taskList);
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<string> GetUserId(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(u => u.UserName == userName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("UserId was not found.");
        }

        private async Task<TaskList> GetTaskList(UpdateTaskListCommand request, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.TaskLists
                .SingleOrDefaultAsync(ti => ti.TaskListId == request.TaskListId, cancellationToken)
                ?? throw new NotFoundException($"TaskListId {request.TaskListId} was not found.");
        }

        private static void UpdateTaskList(UpdateTaskListCommand request, TaskList taskList)
        {
            taskList.TaskListName = request.UpdateTaskListDto.TaskListName;
        }
    }
}

