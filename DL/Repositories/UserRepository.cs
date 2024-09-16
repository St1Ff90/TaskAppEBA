using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly AppEfContext _db;

        public UserRepository(AppEfContext db)
        {
            _db = db;
        }

        public async Task<User> CreateAsync(User user)
        {
            _db.MyUsers.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.MyUsers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _db.MyUsers.ToListAsync();
        }

        public async Task<User> UpdateAsync(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteAsync(User user)
        {
            _db.MyUsers.Remove(user);
            return await _db.SaveChangesAsync() != 0;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.MyUsers.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}
