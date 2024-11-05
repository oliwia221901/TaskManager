using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Application.Common.Interfaces;
using TaskManagerAPI.Persistence.Context;

namespace TaskManagerAPI.Persistence.Services
{
    public class TaskItemAuthorizationService : ITaskItemAuthorizationService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly TaskManagerDbContext _taskManagerDbContext;

        public TaskItemAuthorizationService(ICurrentUserService currentUserService, TaskManagerDbContext taskManagerDbContext)
        {
            _currentUserService = currentUserService;
            _taskManagerDbContext = taskManagerDbContext;
        }

        public async Task AuthorizeAccessToTaskItemAsync(int taskItemId)
        {
            var currentUserName = _currentUserService.GetCurrentUserName();

            var hasAccess = await _taskManagerDbContext.TaskLists
                .AnyAsync(tl => tl.UserName == currentUserName &&
                                tl.TaskItems.Any(ti => ti.TaskItemId == taskItemId));

            if (!hasAccess)
                throw new ForbiddenAccessException($"User '{currentUserName}' does not have access to TaskItemId {taskItemId}.");
        }

        public async Task AuthorizeAccessToTaskListAsync(int taskListId)
        {
            var currentUserName = _currentUserService.GetCurrentUserName();

            var hasAccess = await _taskManagerDbContext.TaskLists
                .AnyAsync(tl => tl.UserName == currentUserName &&
                                tl.TaskListId == taskListId);

            if (!hasAccess)
                throw new ForbiddenAccessException($"User '{currentUserName}' does not have access to TaskListId {taskListId}.");
        }
    }
}
