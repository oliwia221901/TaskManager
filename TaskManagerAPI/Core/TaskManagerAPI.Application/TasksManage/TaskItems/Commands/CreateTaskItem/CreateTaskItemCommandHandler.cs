using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Application.Dtos.CreateTask;
using TaskManagerAPI.Domain.Entities.PermissionManage.Enums;
using TaskManagerAPI.Domain.Entities.TaskManage;

namespace TaskManagerAPI.Application.TasksManage.TaskItems.Commands
{
    public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, int>
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly IAccessControlService _accessControlService;
        private readonly ICurrentUserService _currentUserService;

        public CreateTaskItemCommandHandler(ITaskManagerDbContext taskManagerDbContext, IAccessControlService accessControlService, ICurrentUserService currentUserService)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _accessControlService = accessControlService;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var userName = _currentUserService.GetCurrentUserName();

            var userId = await GetUserId(userName, cancellationToken);

            await EnsureTaskListExists(request.CreateTaskItemDto.TaskListId, cancellationToken);

            await _accessControlService.EnsureUserHasAccess(userId, request.CreateTaskItemDto.TaskListId, PermissionLevel.FullControl, cancellationToken);

            var taskItem = CreateTaskItem(request.CreateTaskItemDto, userId);

            _taskManagerDbContext.TaskItems.Add(taskItem);

            await _taskManagerDbContext.SaveChangesAsync(cancellationToken);

            return taskItem.TaskItemId;
        }

        private async Task<string> GetUserId(string userName, CancellationToken cancellationToken)
        {
            return await _taskManagerDbContext.AppUsers
                .Where(x => x.UserName == userName)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("UserId was not found.");
        }

        private async Task EnsureTaskListExists(int taskListId, CancellationToken cancellationToken)
        {
            var taskListExists = await _taskManagerDbContext.TaskLists
                .AnyAsync(t => t.TaskListId == taskListId, cancellationToken);

            if (!taskListExists)
                throw new NotFoundException($"TaskListId {taskListId} was not found.");
        }

        private static TaskItem CreateTaskItem(CreateTaskItemDto createTaskItemDto, string userId)
        {
            return new TaskItem
            {
                TaskItemName = createTaskItemDto.TaskItemName,
                TaskListId = createTaskItemDto.TaskListId,
                CreatedByUser = userId,
                CreatedAt = DateTime.Now
            };
        }
    }
}
