using HelloApi.Models.DTOs;
using HelloApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelloApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.AuthenticateAsync(request);
            if (response == null)
                return Unauthorized("Invalid username or password");

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result == null)
                return BadRequest("Username already exists or role is invalid");

            return Ok(new { message = result });
        }
    
    }
}
