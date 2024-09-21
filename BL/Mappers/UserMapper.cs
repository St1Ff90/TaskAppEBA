using BL.Models.Requests;
using DAL.Entities;

namespace BL.Mappers
{
    public static class UserMapper
    {
        public static User MapToUserModelRegistrationRequest(RegistrationRequest registrationDto)
        {
            return new User()
            {
                Username = registrationDto.Username!,
                Email = registrationDto.Email!,
                PasswordHash = registrationDto.Password!
            };
        }
    }
}
