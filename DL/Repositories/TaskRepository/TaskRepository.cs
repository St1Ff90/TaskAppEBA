using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories.TaskRepository.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        protected readonly AppEfContext _db;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(AppEfContext db, ILogger<TaskRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<IEnumerable<UserTask>> GetUserTasksAsunc(
            Expression<Func<UserTask, bool>> filterPredicate,
            int pageNumber,
            int pageSize,
            SortField sortBy,
            bool isAsc)
        {
            var tasks = _db.UserTasks.Where(filterPredicate);
            var func = GetSortedItems(sortBy, isAsc);
            var sortedTasks = func(tasks);
            var pagedTasks = sortedTasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var taskList = await pagedTasks.ToListAsync();

            _logger.LogInformation("{TaskCount} tasks retrieved for filter {FilterPredicate}.", taskList.Count, filterPredicate);

            return taskList;
        }

        public async Task<UserTask?> GetUserTaskAsync(Guid userId, Guid id)
        {
            var task = await _db.UserTasks.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (task != null)
            {
                _logger.LogInformation("Task with ID {TaskId} retrieved for UserId {UserId}.", id, userId);
            }
            else
            {
                _logger.LogWarning("Task with ID {TaskId} not found for UserId {UserId}.", id, userId);
            }

            return task;
        }

        public async Task<UserTask> CreateAsync(UserTask task)
        {
            _db.UserTasks.Add(task);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Task with ID {TaskId} created for UserId {UserId}.", task.Id, task.UserId);

            return task;
        }

        public async Task<UserTask> UpdateUserTaskAsync(UserTask task)
        {
            _db.Entry(task).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Task with ID {TaskId} updated for UserId {UserId}.", task.Id, task.UserId);

            return task;
        }

        public async Task<bool> DeleteUserTaskAsync(Guid userId, Guid taskId)
        {
            var affectedRows = await _db.UserTasks
                .Where(t => t.Id == taskId && t.UserId == userId)
                .ExecuteDeleteAsync();

            if (affectedRows > 0)
            {
                _logger.LogInformation("Task with ID {TaskId} deleted for UserId {UserId}.", taskId, userId);
            }
            else
            {
                _logger.LogWarning("Failed to delete Task with ID {TaskId} for UserId {UserId}. Task not found or belongs to another user.", taskId, userId);
            }

            return affectedRows > 0;
        }

        private static Func<IQueryable<UserTask>, IOrderedQueryable<UserTask>>
            GetSortedItems(
            SortField field,
            bool isAscending)
        {
            Expression<Func<UserTask, object>> predicate;
            switch (field)
            {
                case SortField.DueDate:
                    predicate = x => x.DueDate;
                    break;
                case SortField.Priority:
                    predicate = x => x.Priority;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return isAscending ?
                x => x.OrderBy(predicate) :
                x => x.OrderByDescending(predicate);
        }
    }
}