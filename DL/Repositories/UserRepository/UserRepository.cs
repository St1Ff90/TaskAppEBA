using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories.UserRepository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        protected readonly AppEfContext _db;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppEfContext db, ILogger<UserRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<User> CreateAsync(User user)
        {
            _db.MyUsers.Add(user);
            await _db.SaveChangesAsync();

            _logger.LogInformation("User with ID {UserId} created.", user.Id);

            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _db.MyUsers.SingleOrDefaultAsync(x => x.Email == email);

            if (user != null)
            {
                _logger.LogInformation("User with email {Email} retrieved.", email);
            }
            else
            {
                _logger.LogWarning("User with email {Email} not found.", email);
            }

            return user;
        }
    }
}
