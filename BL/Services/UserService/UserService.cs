using BL.Mappers;
using BL.Models.DTO;
using BL.Models.Requests;
using BL.Services.TokenService;
using DAL.Repositories.UserRepository.UserRepository;
using Lection2_Core_BL.Services.HashService;

namespace BL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IHashService hashService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(RegistrationRequest registrationRequest)
        {
            var user = UserMapper.MapToUserModelRegistrationRequest(registrationRequest);
            user.Id = Guid.NewGuid();
            user.PasswordHash = _hashService.GetHash(registrationRequest.Password!);
            var currentDate = DateTime.UtcNow.Date;
            user.CreatedAt = currentDate;
            user.UpdatedAt = currentDate;

            await _userRepository.CreateAsync(user);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email!);
            if (!string.IsNullOrEmpty(loginDto.Password) && user != null)
            {
                if (_hashService.VerifySameHash(loginDto.Password, user.PasswordHash))
                {
                    return _tokenService.GenerateToken(user.Id);
                }
            }

            return string.Empty; //Проверить
        }
    }
}
