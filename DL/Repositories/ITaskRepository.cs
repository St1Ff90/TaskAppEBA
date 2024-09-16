using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITaskRepository
    {
        Task<MyTask> CreateAsync(MyTask task);
        Task<MyTask> UpdateAsync(MyTask task);
        Task<bool> DeleteAsync(MyTask task);
        Task<MyTask?> GetAsync(Guid id);
        Task<ICollection<MyTask>> GetAllAsync();
    }
}
