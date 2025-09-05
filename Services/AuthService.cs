using HelloApi.Models;
using HelloApi.Models.DTOs;
using HelloApi.Repositories.Interfaces;
using HelloApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelloApi.Services
{
    public class AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration config) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _config = config;
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = await _userRepository.GetUserWithRoleAsync(request.Username, hashedPassword);
            if (user == null)
                return null;

            var token = GenerateJwtToken(user);
            RoleDto roleDto = new()
            {
                Name = user.Role.Name
            };
            return new LoginResponseDto
            {
                Username = user.Username,
                Role = roleDto,
                Token = token
            };
        }

        public async Task<string?> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                return null;
            var role = await _roleRepository.GetByNameAsync(request.Role.Name);
            if (role == null)
                return null;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Password = hashedPassword,
                RoleId = role.Id
            };

            await _userRepository.AddAsync(user);
            return "User registered successfully.";
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "YourSuperSecretKey123!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}