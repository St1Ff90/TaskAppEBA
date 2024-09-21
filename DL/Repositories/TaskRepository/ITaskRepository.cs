using Core.Models;
using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Repositories.TaskRepository.TaskRepository
{
    public interface ITaskRepository
    {
        Task<UserTask> CreateAsync(UserTask task);
        Task<UserTask> UpdateUserTaskAsync(UserTask task);
        Task DeleteUserTaskAsync(Guid userId, Guid taskId);
        Task<UserTask?> GetUserTaskAsync(Guid userId, Guid id);
        Task<IEnumerable<UserTask>> GetUserTasksAsunc(Expression<Func<UserTask, bool>> predicate,
            int pageNumber,
            int pageSize,
            SortField sortBy,
            bool byAsc);
    }
}
