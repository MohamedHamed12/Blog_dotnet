using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Tests.IntegrationTests.Controllers;

public class PostsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) 
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetPosts_ReturnsOkResultWithPosts()
    {
        // Arrange
        await SeedDatabase();

        // Act
        var response = await _client.GetAsync("/api/posts");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedPosts = JsonConvert.DeserializeObject<IEnumerable<Post>>(responseContent);

        // Assert
        returnedPosts.Should().HaveCount(2);
        returnedPosts.Should().Contain(post => post.Title == "Post 1");
        returnedPosts.Should().Contain(post => post.Title == "Post 2");
    }

    [Fact]
    public async Task CreatePost_AddsNewPost()
    {
        // Arrange
        var newPost = new Post { Title = "Post 3", Content = "Content 3" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/posts", newPost);
        response.EnsureSuccessStatusCode();

        var createdPost = JsonConvert.DeserializeObject<Post>(await response.Content.ReadAsStringAsync());

        // Assert
        createdPost.Should().NotBeNull();
        createdPost!.Title.Should().Be(newPost.Title);
        createdPost.Content.Should().Be(newPost.Content);
        createdPost.Id.Should().NotBe(0);
    }

    [Fact]
    public async Task DeletePost_RemovesPost()
    {
        // Arrange
        await SeedDatabase();

        // Act
        var response = await _client.DeleteAsync("/api/posts/1");
        response.EnsureSuccessStatusCode();

        // Assert
        var getResponse = await _client.GetAsync("/api/posts/1");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task SeedDatabase()
    {
        var posts = new List<Post>
        {
            new() { Title = "Post 1", Content = "Content 1" },
            new() { Title = "Post 2", Content = "Content 2" }
        };

        var response = await _client.PostAsJsonAsync("/api/posts/seed", posts);
        response.EnsureSuccessStatusCode();
    }
}
// using System.Collections.Generic;
// using System.Net;
// using System.Net.Http;
// using System.Net.Http.Json;
// using System.Threading.Tasks;
// using Core.Entities;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
// using Xunit;

// namespace Tests.IntegrationTests.Controllers
// {
//     public class PostsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
//     {
//         private readonly HttpClient _client;

//         public PostsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
//         {
//             _client = factory.CreateClient();
//             SeedDatabase().GetAwaiter().GetResult();
//         }

//         private async Task SeedDatabase()
//         {
//             var posts = new List<Post>
//             {
//                 new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
//                 new Post { Id = 2, Title = "Post 2", Content = "Content 2" }
//             };

//             // Assume you have an endpoint to seed data for testing
//             var response = await _client.PostAsJsonAsync("/api/posts/seed", posts);
//             response.EnsureSuccessStatusCode();
//         }

//         [Fact]
//         public async Task GetPosts_ReturnsOkResultWithPosts()
//         {
//             // Act
//             var response = await _client.GetAsync("/api/posts");
//             response.EnsureSuccessStatusCode();

//             var responseContent = await response.Content.ReadAsStringAsync();
//             var returnedPosts = JsonConvert.DeserializeObject<IEnumerable<Post>>(responseContent);

//             // Assert
//             returnedPosts.Should().HaveCount(2);
//             returnedPosts.Should().Contain(post => post.Title == "Post 1");
//             returnedPosts.Should().Contain(post => post.Title == "Post 2");
//         }

//         [Fact]
//         public async Task CreatePost_AddsNewPost()
//         {
//             // Arrange
//             var newPost = new Post { Id = 3, Title = "Post 3", Content = "Content 3" };

//             // Act
//             var response = await _client.PostAsJsonAsync("/api/posts", newPost);
//             response.EnsureSuccessStatusCode();

//             var createdPost = JsonConvert.DeserializeObject<Post>(await response.Content.ReadAsStringAsync());

//             // Assert
//             createdPost.Should().BeEquivalentTo(newPost);
//         }

//         [Fact]
//         public async Task DeletePost_RemovesPost()
//         {
//             // Act
//             var response = await _client.DeleteAsync("/api/posts/1");
//             response.EnsureSuccessStatusCode();

//             // Assert
//             var getResponse = await _client.GetAsync("/api/posts/1");
//             getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound); // Assuming the post is not found after deletion
//         }
//     }
// }

// // using System.Collections.Generic;
// // using System.Threading.Tasks;
// // using API.Controllers;
// // using Core.Entities;
// // using Core.Interfaces;
// // using FluentAssertions;
// // using Infrastructure.Data;
// // using Infrastructure.Repositories;
// // using Microsoft.AspNetCore.Mvc;
// // using Microsoft.EntityFrameworkCore;
// // using Microsoft.Extensions.DependencyInjection;
// // using Xunit;

// // namespace Tests.IntegrationTests.Controllers
// // {
// //     public class PostsControllerIntegrationTests
// //     {
// //         private readonly BlogDbContext _context;
// //         private readonly IPostRepository _repository;
// //         private readonly PostsController _controller;
// //         private readonly HttpClient _client;



// //     public PostsControllerIntegrationTests()
// //     {
// //         // Set up in-memory database
// //         var serviceProvider = new ServiceCollection()
// //             .AddEntityFrameworkInMemoryDatabase()
// //             .BuildServiceProvider();

// //         var options = new DbContextOptionsBuilder<BlogDbContext>()
// //             .UseInMemoryDatabase("InMemoryBlogDb")
// //             .UseInternalServiceProvider(serviceProvider)
// //             .Options;

// //         _context = new BlogDbContext(options);
// //         _context.Database.EnsureCreated();

// //         // Create the repository and pass it to the controller
// //         _repository = new PostRepository(_context);
// //         _controller = new PostsController(_repository);

// //         // Reseed the database before each test
// //         _context.Database.EnsureDeleted();
// //         _context.Database.EnsureCreated();
// //         SeedDatabase();
// //     }

// //         // public PostsControllerIntegrationTests()
// //         // {
// //         //     // Set up in-memory database
// //         //     var serviceProvider = new ServiceCollection()
// //         //         .AddEntityFrameworkInMemoryDatabase()
// //         //         .BuildServiceProvider();

// //         //     var options = new DbContextOptionsBuilder<BlogDbContext>()
// //         //         .UseInMemoryDatabase("InMemoryBlogDb")
// //         //         .UseInternalServiceProvider(serviceProvider)
// //         //         .Options;

// //         //     _context = new BlogDbContext(options);
// //         //     _context.Database.EnsureCreated();

// //         //     // Create the repository and pass it to the controller
// //         //     _repository = new PostRepository(_context);
// //         //     _controller = new PostsController(_repository);

// //         //     // Seed the in-memory database with test data
// //         //     SeedDatabase();
// //         // }

// //         // private void SeedDatabase()
// //         // {
// //         //     var posts = new List<Post>
// //         //     {
// //         //         new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
// //         //         new Post { Id = 2, Title = "Post 2", Content = "Content 2" }
// //         //     };

// //         //     _context.Posts.AddRange(posts);
// //         //     _context.SaveChanges();
// //         // }
// // private void SeedDatabase()
// // {
// //     var posts = new List<Post>
// //     {
// //         new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
// //         new Post { Id = 2, Title = "Post 2", Content = "Content 2" }
// //     };

// //     _context.Posts.AddRange(posts);
// //     _context.SaveChanges();

// //     // Diagnostic: Check if the posts are added to the database
// //     var dbPostCount = _context.Posts.Count();
// //     Console.WriteLine($"Number of posts in the database after seeding: {dbPostCount}");
// // }





// //     [Fact]
// //     public async Task GetPosts_ReturnsOkResultWithPosts()
// //     {
// //         // Arrange
// //         var posts = new List<Post>
// //         {
// //             new Post { Id = 3, Title = "Post 1", Content = "Content 1" },
// //             new Post { Id = 4, Title = "Post 2", Content = "Content 2" }
// //         };

// //         // Assuming you have a method to seed your test database
// //         // await SeedDatabase(posts);

// //         // Act
// //         var response = await _client.GetAsync("/api/posts");
// //         response.EnsureSuccessStatusCode();

// //         var responseContent = await response.Content.ReadAsStringAsync();
// //         var returnedPosts = JsonConvert.DeserializeObject<IEnumerable<Post>>(responseContent);

// //         // Assert
// //         returnedPosts.Should().HaveCount(2);
// //         returnedPosts.Should().Contain(post => post.Title == "Post 1");
// //         returnedPosts.Should().Contain(post => post.Title == "Post 2");
// //     }

// //         // [Fact]
// //         // public async Task GetPosts_ReturnsOkResultWithPosts()
// //         // {   

// //         //         var posts = new List<Post>
// //         // {
// //         //     new Post { Id = 3, Title = "Post 1", Content = "Content 1" },
// //         //     new Post { Id = 4, Title = "Post 2", Content = "Content 2" }
// //         // };

// //         // _context.Posts.AddRange(posts);
// //         // _context.SaveChanges();
// //         //     // Act
// //         //     var result = await _controller.GetPosts();

// //         //     // Assert
// //         //     var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
// //         //     var returnedPosts = okResult.Value.Should().BeAssignableTo<IEnumerable<Post>>().Subject;
// //         //     // returnedPosts.Should().HaveCount(2);
// //         // }

// //         // [Fact]
// //         // public async Task GetPost_WithValidId_ReturnsOkResultWithPost()
// //         // {
// //         //     // Act
// //         //     var result = await _controller.GetPost(1);

// //         //     // Assert
// //         //     var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
// //         //     var returnedPost = okResult.Value.Should().BeAssignableTo<Post>().Subject;
// //         //     returnedPost.Id.Should().Be(1);
// //         // }

// //         [Fact]
// //         public async Task CreatePost_AddsNewPost()
// //         {
// //             // Arrange
// //             var newPost = new Post { Id = 3, Title = "Post 3", Content = "Content 3" };

// //             // Act
// //             var result = await _controller.CreatePost(newPost);

// //             // Assert
// //             var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
// //             createdAtActionResult.ActionName.Should().Be(nameof(PostsController.GetPost));
// //             createdAtActionResult.RouteValues["id"].Should().Be(newPost.Id);
// //             createdAtActionResult.Value.Should().BeEquivalentTo(newPost);

// //             // Verify the post was added to the database
// //             var postInDb = await _context.Posts.FindAsync(newPost.Id);
// //             postInDb.Should().NotBeNull();
// //         }

// //         [Fact]
// //         public async Task DeletePost_RemovesPost()
// //         {
// //             // Act
// //             var result = await _controller.DeletePost(1);

// //             // Assert
// //             result.Should().BeOfType<NoContentResult>();

// //             // Verify the post was removed from the database
// //             var postInDb = await _context.Posts.FindAsync(1);
// //             postInDb.Should().BeNull();
// //         }
// //     }
// // }
