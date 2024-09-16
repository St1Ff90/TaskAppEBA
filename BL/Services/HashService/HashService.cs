using Lection2_Core_BL.Services.HashService;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace BL.Services.HashService
{
    public class HashService : IHashService
    {
        public string GetHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
        }

        public bool VerifySameHash(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }
}
