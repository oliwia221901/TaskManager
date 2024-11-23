namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface ITaskAuthorizationService
    {
        Task AuthorizeAccessToTaskItem(int taskItemId, CancellationToken cancellationToken);
        Task AuthorizeAccessToTaskList(int taskListId, CancellationToken cancellationToken);
    }
}
