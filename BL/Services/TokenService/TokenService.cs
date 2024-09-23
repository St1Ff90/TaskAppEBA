using BL.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BL.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private const int TokenLifetimeInHours = 1;
        private readonly AuthOptions _authOptions;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IOptions<AuthOptions> authOptions, ILogger<TokenService> logger)
        {
            _authOptions = authOptions.Value;
            _logger = logger;
        }


        public string GenerateToken(Guid userId)
        {
            _logger.LogInformation("Generating token for user with ID: {UserId}", userId);

            if (string.IsNullOrEmpty(_authOptions.Key))
            {
                _logger.LogError("Token generation failed: AuthOptions key is null or empty.");
                throw new InvalidOperationException("Security key is not configured.");
            }

            try
            {
                var signingKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_authOptions.Key));
                var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                    Expires = DateTime.UtcNow.AddHours(TokenLifetimeInHours),
                    SigningCredentials = signingCredentials
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                _logger.LogInformation("Token generated successfully for user with ID: {UserId}", userId);

                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating token for user with ID: {UserId}", userId);
                throw new Exception("An error occurred while generating the token.");
            }
        }
    }
}
