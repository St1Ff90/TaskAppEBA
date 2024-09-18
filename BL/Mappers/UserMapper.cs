using BL.Requests;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
