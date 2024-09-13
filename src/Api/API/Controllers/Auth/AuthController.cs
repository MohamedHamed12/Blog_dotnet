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
