//using AutoMapper;
using AutoMapper;
using BL.DTO;
using BL.Services.TokenService;
using DAL.Entities;
using DAL.Repositories;
using Lection2_Core_BL.Services.HashService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, IHashService hashService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _mapper = mapper;
            _hashService = hashService;
            _tokenService = tokenService;

        }

        public async Task RegisterAsync(RegistrationDto registrationDto)
        {
            var dto = _mapper.Map<User>(registrationDto);
            var sdsd = _hashService.GetHash(registrationDto.Password!);
            var sdsd1 = _hashService.GetHash(registrationDto.Password!);

            dto.PasswordHash = _hashService.GetHash(registrationDto.Password!);

            await _userRepository.CreateAsync(dto);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email!);
            if (!String.IsNullOrEmpty(loginDto.Password) && user != null)
            {
                if (_hashService.VerifySameHash(loginDto.Password, user.PasswordHash))
                {
                    return _tokenService.GenerateToken(user.Username);
                }
            }

            return string.Empty;
        }
    }
}
