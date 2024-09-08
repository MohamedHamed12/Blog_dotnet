using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ApplicationUser> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                DateOfBirth = registerDto.DateOfBirth,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                return user;
            }
            var errors = result.Errors.Select(e => e.Description).ToList();
            throw new ApiException(
                HttpStatusCode.BadRequest,
                $"User registration failed: {string.Join(", ", errors)}"
            );
            // throw new Exception("User registration failed: " + string.Join(", ", errors));
        }

        public async Task<TokenResponseDto> AuthenticateAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
            {
                throw new ApiException(
                    HttpStatusCode.Unauthorized,
                    "Invalid username or password."
                );
            }

            // Generate tokens
            var accessToken = await GenerateJwtTokenAsync(user, 30); // Access token valid for 30 minutes
            var refreshToken = await GenerateJwtTokenAsync(user, 10080); // Refresh token valid for 7 days (10080 minutes)

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(30),
                User = new UserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    // Add any other user data you want to include
                },
            };
        }

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user, int expiryInMinutes)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
