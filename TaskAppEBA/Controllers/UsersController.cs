using Microsoft.AspNetCore.Mvc;
using BL.Models.Requests;
using BL.Models.DTO;
using BL.Services.UserService;

namespace TaskAppEBA.Controllers
{
    [Route("[controller]")]
    public class UsersController(IUserService _userService, ILogger<UsersController> _logger) : ControllerBase
    {
        /// <summary>
        /// Registers a new user with the specified <paramref name="registrationDto"/>.
        /// Returns a <see cref="System.Net.HttpStatusCode.OK"/> status if the registration is successful,
        /// or a <see cref="System.Net.HttpStatusCode.BadRequest"/> if the input data is invalid.
        /// </summary>
        /// <param name="registrationDto">The request object containing user registration details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistrationRequest registrationDto)
        {
            if (registrationDto == null)
            {
                _logger.LogWarning("Registration data is null.");
                return BadRequest(new { Message = "Registration data cannot be null." });
            }

            await _userService.RegisterAsync(registrationDto);
            _logger.LogInformation("User registered successfully with email: {Email}", registrationDto.Email);
            return Ok();
        }

        /// <summary>
        /// Authenticates a user with the specified <paramref name="loginDto"/>.
        /// Returns a <see cref="System.Net.HttpStatusCode.OK"/> status with the authentication token if the login is successful,
        /// or a <see cref="System.Net.HttpStatusCode.Unauthorized"/> if the credentials are invalid.
        /// </summary>
        /// <param name="loginDto">The request object containing user login credentials.</param>
        /// <returns>An <see cref="IActionResult"/> containing the authentication token or an error response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                _logger.LogWarning("Login data is null.");
                return BadRequest(new { Message = "Login data cannot be null." });
            }

            var token = await _userService.LoginAsync(loginDto);

            if (token == null)
            {
                _logger.LogWarning("Invalid login attempt for Email: {Email}", loginDto.Email);
                return Unauthorized(new { Message = "Invalid username or password." });
            }
            _logger.LogInformation("User logged in successfully with Email: {Email}", loginDto.Email);
            return Ok(token);
        }
    }
}
