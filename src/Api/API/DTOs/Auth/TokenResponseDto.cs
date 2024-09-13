// public class TokenResponseDto
// {
//     public string Token { get; set; }
//     public DateTime Expiration { get; set; }
// }
//
public class TokenResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; } // Expiration of the access token
    public UserDto User { get; set; } // User data
}
