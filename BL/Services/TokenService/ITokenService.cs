using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId);
    }
}
