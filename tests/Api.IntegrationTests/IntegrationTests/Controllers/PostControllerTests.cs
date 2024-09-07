using System.Net.Http.Json;
using System.Text.Json;
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

public class PostControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public PostControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        // _client = factory.CreateClient();
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false }
        );
    }

    // [Fact]
    // public async Task Create_ShouldReturnBadValidation()
    // {
    //     // Arrange
    // }

    [Fact]
    public async Task getPosts_ReturnsOkResultWithPosts()
    {
        // Arrange
        // Act
        for (int i = 0; i < 10; i++)
        {
            // create post


            var randomInt = new Random().Next(0, 1000);
            var newPost = new PostDto
            {
                Title = "Test Post" + randomInt,
                Content = "This is a test post content.",
            };
            await _client.PostAsJsonAsync("/api/Posts", newPost);
        }
        // var response = await _client.GetAsync("/api/Posts?page=1&pageSize=1");
        var response = await _client.GetAsync("/api/Posts?Sorts=Title");
        // Assert

        var content = await response.Content.ReadAsStringAsync();
        // Console.WriteLine(content);
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}
