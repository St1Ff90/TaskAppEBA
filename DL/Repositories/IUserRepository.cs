using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User task);
        Task<User> UpdateAsync(User task);
        Task<bool> DeleteAsync(User task);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<ICollection<User>> GetAllAsync();
    }
}
