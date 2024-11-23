using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.TasksManage.TaskItems.Commands.UpdateTaskItem;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands
{
    public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly IAccessControlService _accessControlService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTaskItemCommandHandler(
            ITaskManagerDbContext taskManagerDbContext,
            IAccessControlService accessControlService,
            ICurrentUserService currentUserService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _accessControlService = accessControlService;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();
            var currentUserId = await GetRequesterId(userName, cancellationToken);

            await _accessControlService.EnsureUserHasAccess(currentUserId, request.UpdateTaskItemDto.TaskItemId, PermissionLevel.ReadWrite, cancellationToken);

            var taskItem = await _taskManagerDbContext.TaskItems
                .SingleOrDefaultAsync(ti => ti.TaskItemId == request.UpdateTaskItemDto.TaskItemId, cancellationToken)
                ?? throw new NotFoundException($"TaskItemId {request.UpdateTaskItemDto.TaskItemId} was not found.");

            taskItem.TaskItemName = request.UpdateTaskItemDto.TaskItemName;

            _taskManagerDbContext.TaskItems.Update(taskItem);
            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<string> GetRequesterId(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(u => u.UserName == userName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Requester was not found.");
        }
    }
}
