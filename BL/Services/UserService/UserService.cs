using BL.Mappers;
using BL.Models.DTO;
using BL.Models.Requests;
using BL.Services.TokenService;
using DAL.Repositories.UserRepository.UserRepository;
using Lection2_Core_BL.Services.HashService;
using Microsoft.Extensions.Logging;

namespace BL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IHashService hashService, ITokenService tokenService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task RegisterAsync(RegistrationRequest registrationRequest)
        {
            if (registrationRequest == null)
            {
                _logger.LogError("Registration failed: registrationRequest is null.");
                throw new ArgumentNullException(nameof(registrationRequest), "Registration request cannot be null.");
            }

            _logger.LogInformation("Attempting to register user with email: {Email}", registrationRequest.Email);

            if (string.IsNullOrEmpty(registrationRequest.Email))
            {
                _logger.LogError("Registration failed: Email is empty.");
                throw new ArgumentException("Email cannot be empty.", nameof(registrationRequest.Email));
            }

            if (string.IsNullOrEmpty(registrationRequest.Password))
            {
                _logger.LogError("Registration failed: Password is empty.");
                throw new ArgumentException("Password cannot be empty.", nameof(registrationRequest.Password));
            }

            var existingUser = await _userRepository.GetByEmailAsync(registrationRequest.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed: User with email {Email} already exists.", registrationRequest.Email);
                throw new InvalidOperationException($"User with email {registrationRequest.Email} already exists.");
            }

            var user = UserMapper.MapToUserModelRegistrationRequest(registrationRequest);
            user.Id = Guid.NewGuid();
            user.PasswordHash = _hashService.GetHash(registrationRequest.Password!);
            var currentDate = DateTime.UtcNow.Date;
            user.CreatedAt = currentDate;
            user.UpdatedAt = currentDate;

            try
            {
                await _userRepository.CreateAsync(user);
                _logger.LogInformation("User registered successfully with email: {Email}", registrationRequest.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user with email: {Email}", registrationRequest.Email);
                throw new Exception("An error occurred while registering the user.");
            }
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                _logger.LogError("Login failed: loginDto is null.");
                throw new ArgumentNullException(nameof(loginDto), "Login request cannot be null.");
            }

            _logger.LogInformation("Attempting login for user with email: {Email}", loginDto.Email);

            if (string.IsNullOrEmpty(loginDto.Email))
            {
                _logger.LogError("Login failed: Email is empty.");
                throw new ArgumentException("Email cannot be empty.", nameof(loginDto.Email));
            }

            if (string.IsNullOrEmpty(loginDto.Password))
            {
                _logger.LogError("Login failed: Password is empty.");
                throw new ArgumentException("Password cannot be empty.", nameof(loginDto.Password));
            }

            var user = await _userRepository.GetByEmailAsync(loginDto.Email!);
            if (user == null)
            {
                _logger.LogWarning("Login failed: No user found with email {Email}.", loginDto.Email);
                return string.Empty;
            }

            var passwordMatch = _hashService.VerifySameHash(loginDto.Password, user.PasswordHash);
            if (!passwordMatch)
            {
                _logger.LogWarning("Login failed: Incorrect password for user with email {Email}.", loginDto.Email);
                return string.Empty;
            }

            var token = _tokenService.GenerateToken(user.Id);
            _logger.LogInformation("Login successful for user with email: {Email}", loginDto.Email);

            return token;
        }
    }
}
