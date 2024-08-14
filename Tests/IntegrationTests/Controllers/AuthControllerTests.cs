using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using API.Controllers;
using Core.Interfaces;
using API.Models;
using Infrastructure.Services;
using Core.Entities;

public class AuthControllerTests
{
    private readonly AuthController _controller;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<TokenService> _mockTokenService;

    public AuthControllerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockTokenService = new Mock<TokenService>(new Mock<IConfiguration>().Object);
        _controller = new AuthController(_mockUserRepository.Object, _mockTokenService.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenValidRequest()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "password"
        };

        _mockUserRepository.Setup(repo => repo.UserExistsAsync(registerRequest.Username, registerRequest.Email))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User registered successfully.", actionResult.Value);
    }

    // [Fact]
    // public async Task Login_ShouldReturnToken_WhenValidCredentials()
    // {

    //     var registerRequest = new RegisterRequest
    //     {
    //         Username = "testuser1",
    //         Email = "testuser1@example.com",
    //         Password = "password"
    //     };


    //     // Act
    //     var result = await _controller.Register(registerRequest);
    //     // Arrange
    //     var authRequest = new AuthRequest
    //     {
    //         Username = "testuser1",
    //         Password = "password1"
    //     };

    //     var user = new User { Username = "testuser1", Password = "hashedpassword" };
    //     _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(authRequest.Username))
    //         .ReturnsAsync(user);
    //     _mockTokenService.Setup(ts => ts.GenerateToken(user))
    //         .Returns("jwt-token");

    //     // Act
    //      result = await _controller.Login(authRequest);

    //     // Assert
    //     var actionResult = Assert.IsType<OkObjectResult>(result);
    //     var response = Assert.IsType<dynamic>(actionResult.Value);
    //     Assert.Equal("jwt-token", response.token.ToString());
    // }
}
