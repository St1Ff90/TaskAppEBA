using DAL.Entities;

namespace DAL.Repositories.UserRepository.UserRepository
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
