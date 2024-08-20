using System.Net.Http.Json;
using System.Threading.Tasks;
using API;
using API.Models;
using Core.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        // _client = factory.CreateClient();
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });


    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenUserIsCreated()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Password = "StrongPassword123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("User registered successfully.");
    }

//     [Fact]
//     public async Task Register_ShouldReturnBadRequest_WhenUsernameOrEmailIsTaken()
//     {
//         // Arrange
//         var registerRequest = new RegisterRequest
//         {
//             Username = "existinguser",
//             Email = "existinguser@example.com",
//             Password = "StrongPassword123"
//         };

//         // Pre-seed user in the database (or assume one exists)
//         // SeedExistingUser(); // You need to set up this user in your test environment

//         // Act
//         var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

//         // Assert
//         response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
//         var content = await response.Content.ReadAsStringAsync();
//         content.Should().Be("Username or email already taken.");
//     }

//     // [Fact]
//     // public async Task Login_ShouldReturnOk_WithValidCredentials()
//     // {
//     //     // Arrange
//     //     var authRequest = new AuthRequest
//     //     {
//     //         Username = "existinguser",
//     //         Password = "CorrectPassword"
//     //     };

//     //     // Act
//     //     var response = await _client.PostAsJsonAsync("/api/auth/login", authRequest);

//     //     // Assert
//     //     response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//     //     var tokens = await response.Content.ReadFromJsonAsync<TokenResponse>();
//     //     tokens.Should().NotBeNull();
//     //     tokens!.AccessToken.Should().NotBeNullOrEmpty();
//     //     tokens.RefreshToken.Should().NotBeNullOrEmpty();
//     // }

//     [Fact]
//     public async Task Login_ShouldReturnUnauthorized_WithInvalidCredentials()
//     {
//         // Arrange
//         var authRequest = new AuthRequest
//         {
//             Username = "existinguser",
//             Password = "WrongPassword"
//         };

//         // Act
//         var response = await _client.PostAsJsonAsync("/api/auth/login", authRequest);

//         // Assert
//         response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
//         var content = await response.Content.ReadAsStringAsync();
//         content.Should().Be("Invalid username or password.");
//     }

//     // [Fact]
//     // public async Task RefreshToken_ShouldReturnOk_WithValidToken()
//     // {
//     //     // Arrange
//     //     var refreshTokenRequest = new RefreshTokenRequest
//     //     {
//     //         RefreshToken = "ValidRefreshToken"
//     //     };

//     //     // Act
//     //     var response = await _client.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenRequest);

//     //     // Assert
//     //     response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//     //     var tokens = await response.Content.ReadFromJsonAsync<TokenResponse>();
//     //     tokens.Should().NotBeNull();
//     //     tokens!.AccessToken.Should().NotBeNullOrEmpty();
//     //     tokens.RefreshToken.Should().NotBeNullOrEmpty();
//     // }

//     [Fact]
//     public async Task RefreshToken_ShouldReturnUnauthorized_WithInvalidToken()
//     {
//         // Arrange
//         var refreshTokenRequest = new RefreshTokenRequest
//         {
//             RefreshToken = "InvalidRefreshToken"
//         };

//         // Act
//         var response = await _client.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenRequest);

//         // Assert
//         response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
//         var content = await response.Content.ReadAsStringAsync();
//         content.Should().Be("Invalid or expired refresh token.");
//     }
// }

// // using Microsoft.AspNetCore.Mvc;
// // using Moq;
// // using System.Threading.Tasks;
// // using Xunit;
// // using API.Controllers;
// // using Core.Interfaces;
// // using API.Models;
// // using Infrastructure.Services;
// // using Core.Entities;
// // using Microsoft.Extensions.Configuration;

// // public class AuthControllerTests
// // {
// //     private readonly AuthController _controller;
// //     private readonly Mock<IUserRepository> _mockUserRepository;
// //     private readonly Mock<TokenService> _mockTokenService;

// //     public AuthControllerTests()
// //     {
// //         _mockUserRepository = new Mock<IUserRepository>();
// //         _mockTokenService = new Mock<TokenService>(new Mock<IConfiguration>().Object);
// //         _controller = new AuthController(_mockUserRepository.Object, _mockTokenService.Object);
// //     }

// //     [Fact]
// //     public async Task Register_ShouldReturnOk_WhenValidRequest()
// //     {
// //         // Arrange
// //         var registerRequest = new RegisterRequest
// //         {
// //             Username = "testuser",
// //             Email = "testuser@example.com",
// //             Password = "password"
// //         };

// //         _mockUserRepository.Setup(repo => repo.UserExistsAsync(registerRequest.Username, registerRequest.Email))
// //             .ReturnsAsync(false);

// //         // Act
// //         var result = await _controller.Register(registerRequest);

// //         // Assert
// //         var actionResult = Assert.IsType<OkObjectResult>(result);
// //         Assert.Equal("User registered successfully.", actionResult.Value);
// //     }

// //     [Fact]
// //     public async Task Login_ShouldReturnToken_WhenValidCredentials()
// //     {

// //         var registerRequest = new RegisterRequest
// //         {
// //             Username = "testuser1",
// //             Email = "testuser1@example.com",
// //             Password = "password"
// //         };


// //         // Act
// //         var result = await _controller.Register(registerRequest);
// //         // Arrange
// //         var authRequest = new AuthRequest
// //         {
// //             Username = "testuser1",
// //             Password = "password1"
// //         };

// //         var user = new User { Username = "testuser1", Password = "hashedpassword" };
// //         // _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(authRequest.Username))
// //         //     .ReturnsAsync(user);
// //         // _mockTokenService.Setup(ts => ts.GenerateTokens(user))
// //         //     .Returns("");

// //         // Act
// //          result = await _controller.Login(authRequest);

// //         // Assert
// //         var actionResult = Assert.IsType<OkObjectResult>(result);
// //         var response = Assert.IsType<dynamic>(actionResult.Value);
// //         Assert.Equal("jwt-token", response.token.ToString());
// //     }
}
