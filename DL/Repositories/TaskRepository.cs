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

        public async Task<ICollection<MyTask>> GetAllAsync()
        {
            return await _db.MyTasks.ToListAsync();
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
