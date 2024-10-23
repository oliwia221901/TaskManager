using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TaskManagerAPI.Domain.Entities;

namespace TaskManagerAPI.Application.Common.Interfaces
{
    public interface ITaskManagerDbContext
	{
        public DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

