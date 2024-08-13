using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Core.Entities;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests.Controllers
{
    public class PostsControllerTests
    {
        private readonly Mock<IPostRepository> _mockRepository;
        private readonly PostsController _controller;

        public PostsControllerTests()
        {
            _mockRepository = new Mock<IPostRepository>();
            _controller = new PostsController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetPosts_ReturnsOkResultWithPosts()
        {
            // Arrange
            var posts = new List<Post> { new Post(), new Post() };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(posts);

            // Act
            var result = await _controller.GetPosts();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedPosts = okResult.Value.Should().BeAssignableTo<IEnumerable<Post>>().Subject;
            returnedPosts.Should().BeEquivalentTo(posts);
        }

        [Fact]
        public async Task GetPost_WithValidId_ReturnsOkResultWithPost()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = postId };
            _mockRepository.Setup(repo => repo.GetByIdAsync(postId)).ReturnsAsync(post);

            // Act
            var result = await _controller.GetPost(postId);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedPost = okResult.Value.Should().BeAssignableTo<Post>().Subject;
            returnedPost.Should().BeEquivalentTo(post);
        }

        [Fact]
        public async Task GetPost_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var postId = 1;
            _mockRepository.Setup(repo => repo.GetByIdAsync(postId)).ReturnsAsync((Post)null);

            // Act
            var result = await _controller.GetPost(postId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreatePost_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var post = new Post { Id = 1 };
            _mockRepository.Setup(repo => repo.AddAsync(post)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreatePost(post);

            // Assert
            var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be(nameof(PostsController.GetPost));
            createdAtActionResult.RouteValues["id"].Should().Be(post.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(post);
        }

        [Fact]
        public async Task UpdatePost_WithValidIdAndPost_ReturnsNoContent()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = postId };
            _mockRepository.Setup(repo => repo.UpdateAsync(post)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdatePost(postId, post);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdatePost_WithMismatchedId_ReturnsBadRequest()
        {
            // Arrange
            var postId = 1;
            var post = new Post { Id = 2 };

            // Act
            var result = await _controller.UpdatePost(postId, post);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeletePost_ReturnsNoContent()
        {
            // Arrange
            var postId = 1;
            _mockRepository.Setup(repo => repo.DeleteAsync(postId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeletePost(postId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Newtonsoft.Json;
// using Xunit;
// using API;
// using Core.Entities;
// using System.Collections.Generic;
// using Microsoft.AspNetCore.Http;
// using Infrastructure.Data;

// public class PostsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client;
//     private readonly WebApplicationFactory<Program> _factory;

//     public PostsControllerIntegrationTests(WebApplicationFactory<Program> factory)
//     {
//         _factory = factory;
//         _client = factory.WithWebHostBuilder(builder =>
//         {
//             builder.ConfigureServices(services =>
//             {
//                 // Remove the existing DbContext registration
//                 var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlogDbContext>));
//                 if (descriptor != null)
//                 {
//                     services.Remove(descriptor);
//                 }

//                 // Add an in-memory database for testing
//                 services.AddDbContext<BlogDbContext>(options =>
//                 {
//                     options.UseInMemoryDatabase("InMemoryDbForTesting");
//                 });
//             });
//         }).CreateClient();

//         // Ensure the database is created and setup correctly
//         EnsureDatabaseSetup();
//     }

//     private void EnsureDatabaseSetup()
//     {
//         using var scope = _factory.Services.CreateScope();
//         var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
//         dbContext.Database.EnsureCreated();
        
//         // Optionally seed the database here
//         dbContext.Posts.AddRange(new List<Post>
//         {
//             new Post { Id = 1, Title = "Test Post 1", Content = "Content for post 1" },
//             new Post { Id = 2, Title = "Test Post 2", Content = "Content for post 2" }
//         });
//         dbContext.SaveChanges();
//     }

//     [Fact]
//     public async Task GetPosts_ReturnsSuccessStatusCode()
//     {
//         // Act
//         var response = await _client.GetAsync("/api/posts");

//         // Assert
//         response.EnsureSuccessStatusCode();
//     }

//     [Fact]
//     public async Task GetPost_ReturnsSuccessStatusCode_WhenPostExists()
//     {
//         // Act
//         var response = await _client.GetAsync("/api/posts/1");

//         // Assert
//         response.EnsureSuccessStatusCode();
//     }

//     [Fact]
//     public async Task CreatePost_ReturnsCreatedStatusCode()
//     {
//         // Arrange
//         var post = new { Title = "New Post", Content = "Content for new post" };
//         var content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");

//         // Act
//         var response = await _client.PostAsync("/api/posts", content);

//         // Assert
//         Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
//     }

//     [Fact]
//     public async Task UpdatePost_ReturnsNoContentStatusCode()
//     {
//         // Arrange
//         var post = new { Id = 1, Title = "Updated Post", Content = "Updated content" };
//         var content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");

//         // Act
//         var response = await _client.PutAsync("/api/posts/1", content);

//         // Assert
//         Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
//     }

//     [Fact]
//     public async Task DeletePost_ReturnsNoContentStatusCode()
//     {
//         // Act
//         var response = await _client.DeleteAsync("/api/posts/1");

//         // Assert
//         Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
//     }
// }
