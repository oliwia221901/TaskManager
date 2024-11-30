using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskLists.Commands.DeleteTaskList
{
    public class DeleteTaskListCommandHandler : IRequestHandler<DeleteTaskListCommand>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccessControlService _accessControlService;

        public DeleteTaskListCommandHandler(ITaskManagerDbContext taskManagerDbContext, ICurrentUserService currentUserService, IAccessControlService accessControlService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _currentUserService = currentUserService;
            _accessControlService = accessControlService;
        }

        public async Task<Unit> Handle(DeleteTaskListCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            var userId = await GetUserId(userName, cancellationToken);

            await _accessControlService.CheckAccess(userId, request.TaskListId, PermissionLevel.FullControl, true, cancellationToken);

            var taskList = await GetTaskList(request.TaskListId, cancellationToken);

            _taskManagerDbContext.TaskLists.Remove(taskList);
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

        private async Task<TaskList> GetTaskList(int taskListId, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.TaskLists
                .SingleOrDefaultAsync(x => x.TaskListId == taskListId, cancellationToken)
                ?? throw new NotFoundException("TaskListId was not found.");
        }
    }
}
