using Microsoft.AspNetCore.Mvc;
using BL.DTO;
using BL.Services;

namespace TaskAppEBA.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService authService)
        {
            _userService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistrationDto registrationDto)
        {
            await _userService.RegisterAsync(registrationDto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            return Ok(await _userService.LoginAsync(loginDto));
        }
    }
}
