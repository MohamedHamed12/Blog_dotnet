using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtTokenAsync(ApplicationUser user, int expiryInMinutes);

        // Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        Task<TokenResponseDto> AuthenticateAsync(string userName, string password);
        Task<ApplicationUser> RegisterAsync(RegisterDto registerDto);
    }
}
