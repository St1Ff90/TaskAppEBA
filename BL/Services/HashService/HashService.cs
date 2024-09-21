using Lection2_Core_BL.Services.HashService;

namespace BL.Services.HashService
{
    public class HashService : IHashService
    {
        private const int workFactor = 12;

        public string GetHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor);
        }

        public bool VerifySameHash(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }
}
