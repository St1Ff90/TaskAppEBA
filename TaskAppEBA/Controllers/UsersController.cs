using Microsoft.AspNetCore.Mvc;
using BL.Models.Requests;
using BL.Models.DTO;
using BL.Services.UserService;

namespace TaskAppEBA.Controllers
{
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistrationRequest registrationDto)
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
