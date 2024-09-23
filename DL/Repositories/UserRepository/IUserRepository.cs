using DAL.Entities;

namespace DAL.Repositories.UserRepository.UserRepository
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User task);
        Task<User?> GetByEmailAsync(string email);
    }
}
