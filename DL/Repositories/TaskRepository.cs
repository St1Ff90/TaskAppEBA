using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        protected readonly AppEfContext _db;

        public TaskRepository(AppEfContext db)
        {
            _db = db;
        }

        public async Task<MyTask> CreateAsync(MyTask task)
        {
            _db.MyTasks.Add(task);
            await _db.SaveChangesAsync();

            return task;
        }

        public async Task<MyTask?> GetAsync(Guid id)
        {
            return await _db.MyTasks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<MyTask> GetUserTasks(Guid userId, int? status, DateTime? dueDate, int? priority)
        {
            var tasks = _db.MyTasks.Where(x => x.UserId == userId);

            if (status.HasValue)
            {
                tasks = tasks.Where(t => t.Status == status);
            }

            if (dueDate.HasValue)
            {
                tasks = tasks.Where(t => t.DueDate != null && t.DueDate.Value.Date == dueDate.Value.Date);
            }

            if (priority.HasValue)
            {
                tasks = tasks.Where(t => t.Priority == priority.Value);
            }

            return tasks;
        }

        public async Task<IEnumerable<MyTask>> GetAllAsync(Guid userId)
        {
            var sdsds = await _db.MyTasks.ToListAsync();

            return sdsds;
        }

        public async Task<MyTask> UpdateAsync(MyTask task)
        {
            _db.Entry(task).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteAsync(MyTask task)
        {
            _db.MyTasks.Remove(task);
            return await _db.SaveChangesAsync() != 0;
        }
    }
}
