using Microsoft.AspNetCore.Mvc;
using TimesheetSystem.Appliication.Dtos;
using TimesheetSystem.Appliication.IServices;
using log4net;

namespace TimesheetSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UserController));

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            _logger.Info("Register endpoint called");
            try
            {
                var result = await _userService.Register(dto);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = result.ExpiresOn
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error("Error in Register", ex);
                return StatusCode(500, "An unexpected error occurred while registering the user.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            _logger.Info("Login endpoint called");
            try
            {
                var result = await _userService.LoginAsync(dto);

                if (!result.IsAuthenticated)
                    return Unauthorized(result.Message);

                Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = result.ExpiresOn
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error("Error in Login", ex);
                return StatusCode(500, "An unexpected error occurred while logging in.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.Info("Logout endpoint called");
            try
            {
                await _userService.SignOut();
                Response.Cookies.Delete("AuthToken");
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error in Logout", ex);
                return StatusCode(500, "An unexpected error occurred while logging out.");
            }
        }
    }
}
