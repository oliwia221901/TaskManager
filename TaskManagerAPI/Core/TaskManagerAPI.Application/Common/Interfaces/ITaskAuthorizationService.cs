namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface ITaskAuthorizationService
    {
        Task AuthorizeAccessToTaskItem(int taskItemId);
        Task AuthorizeAccessToTaskList(int taskListId);
    }
}
