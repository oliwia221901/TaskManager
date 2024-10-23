using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Application.Common.Exceptions;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Domain.Repository;
using TaskManagerAPI.Persistence.Context;

namespace TaskManagerAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagerDbContext _taskManagerDbContext;

        public UserRepository(TaskManagerDbContext taskManagerDbContext)
        {
            _taskManagerDbContext = taskManagerDbContext;
        }
    }
}
