using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TaskManagerAPI.Domain.Entities.TaskItem;
using TaskManagerAPI.Domain.Entities.UserManage;

namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface ITaskManagerDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<TaskItem> TaskItems { get; set; }
        DbSet<TaskList> TaskLists { get; set; }
        DbSet<AppUser> AppUsers { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
