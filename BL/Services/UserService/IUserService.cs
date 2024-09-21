using BL.Models.DTO;
using BL.Models.Requests;

namespace BL.Services.UserService
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginDto loginDto);
        Task RegisterAsync(RegistrationRequest registrationRequest);
    }
}