using Lection2_Core_BL.Services.HashService;
using Microsoft.Extensions.Logging;

namespace BL.Services.HashService
{
    public class HashService : IHashService
    {
        private const int WorkFactor = 12;
        private readonly ILogger<HashService> _logger;

        public HashService(ILogger<HashService> logger)
        {
            _logger = logger;
        }

        public string GetHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogError("Hash generation failed: password is null or empty.");
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));
            }

            try
            {
                _logger.LogInformation("Generating hash for a password.");
                var hash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
                _logger.LogInformation("Hash generated successfully.");
                return hash;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating hash for the password.");
                throw new Exception("An error occurred while generating hash.");
            }
        }

        public bool VerifySameHash(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
            {
                _logger.LogError("Hash verification failed: password or hash is null or empty.");
                throw new ArgumentException("Password and hash cannot be null or empty.");
            }

            try
            {
                _logger.LogInformation("Verifying password hash.");
                var isMatch = BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
                _logger.LogInformation("Hash verification result: {IsMatch}", isMatch);
                return isMatch;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying password hash.");
                throw new Exception("An error occurred while verifying hash.");
            }
        }
    }
}
