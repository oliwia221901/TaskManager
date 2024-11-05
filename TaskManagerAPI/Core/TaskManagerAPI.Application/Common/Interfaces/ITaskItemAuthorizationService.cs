namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface ITaskItemAuthorizationService
    {
        Task AuthorizeAccessToTaskItemAsync(int taskItemId);
        Task AuthorizeAccessToTaskListAsync(int taskListId);
    }
}

