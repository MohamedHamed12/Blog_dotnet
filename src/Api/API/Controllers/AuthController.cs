using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
// using Infrastructure.Services;
using API.Models;
using BCrypt.Net;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        private readonly BlogDbContext _context;

        public AuthController(
            BlogDbContext context,
            IUserRepository userRepository,
            TokenService tokenService
        )
        {
            _context = context;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userRepository.UserExistsAsync(request.Username, request.Email))
            {
                // Console.WriteLine("*******************"+request.Username);
                // return BadRequest("Username or email already taken.");
                // throw new BadRequestException("Username or email already taken.");
                throw new ApiException(
                    HttpStatusCode.BadRequest,
                    "Username or email already taken."
                );
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = HashPassword(request.Password),
            };

            await _userRepository.AddUserAsync(user);
            var response = new { Success = true, Message = "User created successfully" };
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.Username == request.Username
            );
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                // return Unauthorized("Invalid username or password.");
                throw new ApiException(
                    HttpStatusCode.Unauthorized,
                    "Invalid username or password."
                );
            }
            // var (accessToken, refreshToken) = _tokenService.GenerateTokens(user);
            var (accessToken, refreshToken) = _tokenService.GenerateTokens(
                user.Id.ToString(),
                user.Username
            );

            // Save refresh token and its expiry date to the database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7); // Refresh token expires in 7 days
            await _userRepository.UpdateUserAsync(user);

            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.Now)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            // var (accessToken, newRefreshToken) = _tokenService.GenerateTokens(user);
            var (accessToken, newRefreshToken) = _tokenService.GenerateTokens(
                user.Id.ToString(),
                user.Username
            );

            // Update refresh token in the database
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await _userRepository.UpdateUserAsync(user);

            return Ok(new { accessToken, refreshToken = newRefreshToken });
        }

        private string HashPassword(string password)
        {
            // using (var sha256 = SHA256.Create())
            // {
            //     var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            //     return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            // }
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }
    }
}
