// using Core.Entities;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Security.Cryptography;
// using System.Text;

// namespace Infrastructure.Services
// {
//     public class TokenService
//     {
//         private readonly IConfiguration _configuration;

//         public TokenService(IConfiguration configuration)
//         {
//             _configuration = configuration;
//         }

//         public (string AccessToken, string RefreshToken) GenerateTokens(User user)
//         {
//             // user.Id.ToString(), user.Username
//             var accessToken = GenerateAccessToken(user);
//             Console.WriteLine(accessToken);
//             var refreshToken = GenerateRefreshToken();

//             return (AccessToken: accessToken, RefreshToken: refreshToken);
//         }

//         private string GenerateAccessToken(User user)
//         {
//             var claims = new[]
//             {
//                 new Claim(JwtRegisteredClaimNames.Sub, user.Username),
//                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                 new Claim(ClaimTypes.Name, user.Username)
//             };

//             var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//             var token = new JwtSecurityToken(
//                 issuer: _configuration["Jwt:Issuer"],
//                 audience: _configuration["Jwt:Audience"],
//                 claims: claims,
//                 expires: DateTime.Now.AddMinutes(15), // Access token expires in 15 minutes
//                 signingCredentials: creds
//             );

//             return new JwtSecurityTokenHandler().WriteToken(token);
//         }

//         private string GenerateRefreshToken()
//         {
//             return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)); // Generate a random refresh token
//         }
//     }
// }
// using Microsoft.IdentityModel.Tokens;
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Security.Cryptography;
// using System.Text;
//
// public class TokenService
// {
//     private readonly IConfiguration _configuration;
//
//     public TokenService(IConfiguration configuration)
//     {
//         _configuration = configuration;
//     }
//
//     public (string accessToken, string refreshToken) GenerateTokens(string userId, string username)
//     {
//         var accessToken = GenerateAccessToken(userId, username);
//         Console.WriteLine(accessToken);
//         var refreshToken = GenerateRefreshToken();
//
//         // TODO: Save the refresh token in your database, associated with the user
//
//         return (accessToken, refreshToken);
//     }
//
//     private string GenerateAccessToken(string userId, string username)
//     {
//         var claims = new[]
//         {
//             new Claim(ClaimTypes.NameIdentifier, userId),
//             new Claim(ClaimTypes.Name, username)
//         };
//
//         // var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mySecretKeymySecretKeymySecretKey"));
//         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//
//         var token = new JwtSecurityToken(
//             issuer: _configuration["Jwt:Issuer"],
//             audience: _configuration["Jwt:Audience"],
//             claims: claims,
//             expires: DateTime.Now.AddMinutes(15), // Short-lived access token
//             signingCredentials: creds
//         );
//
//         return new JwtSecurityTokenHandler().WriteToken(token);
//     }
//
//     private string GenerateRefreshToken()
//     {
//         var randomNumber = new byte[32];
//         using (var rng = RandomNumberGenerator.Create())
//         {
//             rng.GetBytes(randomNumber);
//             return Convert.ToBase64String(randomNumber);
//         }
//     }
//
//     public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
//     {
//         var tokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateAudience = false,
//             ValidateIssuer = false,
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
//             ValidateLifetime = false
//         };
//
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
//         if (securityToken is not JwtSecurityToken jwtSecurityToken ||
//             !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
//             throw new SecurityTokenException("Invalid token");
//
//         return principal;
//     }
// }

