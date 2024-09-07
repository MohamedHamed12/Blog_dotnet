// API/Controllers/AuthController.cs
using Core.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator
        )
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var user = await _authService.RegisterAsync(registerDto);
            return Ok(new { UserId = user.Id });
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var validationResult = await _loginValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var tokenResponse = await _authService.AuthenticateAsync(
                loginDto.UserName,
                loginDto.Password
            );
            return Ok(tokenResponse);
        }
    }
}



// using System.Threading.Tasks;
// using BlogBackend.API.DTOs;
// using Core.Entities;
// // using Infrastructure.Services; // Make sure to have a TokenService for handling JWTs
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
//
// [ApiController]
// [Route("api/[controller]")]
// public class AuthController : ControllerBase
// {
//     private readonly UserManager<User> _userManager;
//     private readonly SignInManager<User> _signInManager;
//     private readonly TokenService _tokenService; // Custom service for generating JWTs
//     private readonly IConfiguration _configuration;
//
//     public AuthController(
//         UserManager<User> userManager,
//         SignInManager<User> signInManager,
//         TokenService tokenService,
//         IConfiguration configuration
//     )
//     {
//         _userManager = userManager;
//         _signInManager = signInManager;
//         _tokenService = tokenService;
//         _configuration = configuration;
//     }
//
//     [HttpPost("register")]
//     public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);
//
//         var user = new User
//         {
//             UserName = registerRequest.UserName,
//             Email = registerRequest.Email,
//             ProfilePictureUrl =
//                 registerRequest.ProfilePictureUrl // Optional field
//             ,
//         };
//
//         var result = await _userManager.CreateAsync(user, registerRequest.Password);
//         if (result.Succeeded)
//         {
//             return Ok(new { Message = "User registered successfully" });
//         }
//
//         return BadRequest(result.Errors);
//     }
//
//     [HttpPost("login")]
//     public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);
//
//         var result = await _signInManager.PasswordSignInAsync(
//             loginRequest.UserName,
//             loginRequest.Password,
//             isPersistent: false,
//             lockoutOnFailure: false
//         );
//
//         if (result.Succeeded)
//         {
//             var user = await _userManager.FindByNameAsync(loginRequest.UserName);
//             var token = _tokenService.GenerateToken(user);
//             return Ok(new LoginResponseDTO { Token = token, UserName = user.UserName });
//         }
//
//         return Unauthorized();
//     }
//
//     [HttpPost("refresh")]
//     public IActionResult Refresh([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
//     {
//         var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenRequest.Token);
//         if (principal == null)
//             return BadRequest("Invalid token");
//
//         var username = principal.Identity.Name;
//         var user = _userManager.FindByNameAsync(username).Result;
//         if (user == null)
//             return BadRequest("User not found");
//
//         var newToken = _tokenService.GenerateToken(user);
//         return Ok(new { Token = newToken });
//     }
// }

// using System;
// using System.Net;
// using System.Security.Cryptography;
// using System.Text;
// using System.Threading.Tasks;
// // using Infrastructure.Services;
// using API.Models;
// using BCrypt.Net;
// using Core.Entities;
// using Core.Interfaces;
// using FluentValidation;
// using Infrastructure.Data;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace API.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class AuthController : ControllerBase
//     {
//         private readonly IUserRepository _userRepository;
//         private readonly TokenService _tokenService;
//
//         private readonly BlogDbContext _context;
//
//         public AuthController(
//             BlogDbContext context,
//             IUserRepository userRepository,
//             TokenService tokenService
//         )
//         {
//             _context = context;
//             _userRepository = userRepository;
//             _tokenService = tokenService;
//         }
//
//         [HttpPost("register")]
//         public async Task<IActionResult> Register([FromBody] RegisterRequest request)
//         {
//             if (await _userRepository.UserExistsAsync(request.Username, request.Email))
//             {
//                 // Console.WriteLine("*******************"+request.Username);
//                 // return BadRequest("Username or email already taken.");
//                 // throw new BadRequestException("Username or email already taken.");
//                 throw new ApiException(
//                     HttpStatusCode.BadRequest,
//                     "Username or email already taken."
//                 );
//             }
//
//             var user = new User
//             {
//                 UserName = request.Username,
//                 Email = request.Email,
//                 Password = HashPassword(request.Password),
//             };
//
//             await _userRepository.AddUserAsync(user);
//             var response = new { Success = true, Message = "User created successfully" };
//             return Ok(response);
//         }
//
//         [HttpPost("login")]
//         public async Task<IActionResult> Login([FromBody] AuthRequest request)
//         {
//             var user = await _context.Users.SingleOrDefaultAsync(u =>
//                 u.Username == request.Username
//             );
//             if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
//             {
//                 // return Unauthorized("Invalid username or password.");
//                 throw new ApiException(
//                     HttpStatusCode.Unauthorized,
//                     "Invalid username or password."
//                 );
//             }
//             // var (accessToken, refreshToken) = _tokenService.GenerateTokens(user);
//             var (accessToken, refreshToken) = _tokenService.GenerateTokens(
//                 user.Id.ToString(),
//                 user.Username
//             );
//
//             // Save refresh token and its expiry date to the database
//             user.RefreshToken = refreshToken;
//             user.RefreshTokenExpiry = DateTime.Now.AddDays(7); // Refresh token expires in 7 days
//             await _userRepository.UpdateUserAsync(user);
//
//             return Ok(new { accessToken, refreshToken });
//         }
//
//         [HttpPost("refresh-token")]
//         public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
//         {
//             var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken);
//
//             if (user == null || user.RefreshTokenExpiry < DateTime.Now)
//             {
//                 return Unauthorized("Invalid or expired refresh token.");
//             }
//
//             // var (accessToken, newRefreshToken) = _tokenService.GenerateTokens(user);
//             var (accessToken, newRefreshToken) = _tokenService.GenerateTokens(
//                 user.Id.ToString(),
//                 user.Username
//             );
//
//             // Update refresh token in the database
//             user.RefreshToken = newRefreshToken;
//             user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
//             await _userRepository.UpdateUserAsync(user);
//
//             return Ok(new { accessToken, refreshToken = newRefreshToken });
//         }
//
//         private string HashPassword(string password)
//         {
//             // using (var sha256 = SHA256.Create())
//             // {
//             //     var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//             //     return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
//             // }
//             return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
//         }
//
//         private bool VerifyPassword(string password, string storedHash)
//         {
//             var hashedPassword = HashPassword(password);
//             return hashedPassword == storedHash;
//         }
//     }
// }
