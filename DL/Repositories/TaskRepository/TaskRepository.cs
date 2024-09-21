using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Core.Models;

namespace DAL.Repositories.TaskRepository.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        protected readonly AppEfContext _db;

        public TaskRepository(AppEfContext db)
        {
            _db = db;
        }

        public async Task<UserTask> CreateAsync(UserTask task)
        {
            _db.MyTasks.Add(task);
            await _db.SaveChangesAsync();

            return task;
        }

        public async Task<UserTask?> GetUserTaskAsync(Guid userId, Guid id)
        {
            return await _db.MyTasks.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }

        public async Task<UserTask> UpdateUserTaskAsync(UserTask task)
        {
            _db.Entry(task).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return task;
        }

        public async Task DeleteUserTaskAsync(Guid userId, Guid taskId)
        {
            var task = new UserTask { Id = taskId, UserId = userId };

            _db.MyTasks.Entry(task).State = EntityState.Deleted;

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserTask>> GetUserTasksAsunc(
            Expression<Func<UserTask, bool>> filterPredicate,
            int pageNumber,
            int pageSize,
            SortField sortBy,//enum
            bool sortDirection)//bool ascending
        {
            var tasks = _db.MyTasks.Where(filterPredicate);

            var func = GetSortedItems(sortBy, sortDirection);
            var query = func(tasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)).ToQueryString();
            return await
                func(tasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize))
                .ToListAsync();
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
                case SortField.Status:
                    predicate = x => x.Status;
                    break;
                case SortField.Title:
                    predicate = x => x.Title;
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