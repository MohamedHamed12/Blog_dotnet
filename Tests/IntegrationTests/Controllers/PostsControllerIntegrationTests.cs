using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Core.Entities;
using Core.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.IntegrationTests.Controllers
{
    public class PostsControllerIntegrationTests
    {
        private readonly BlogDbContext _context;
        private readonly IPostRepository _repository;
        private readonly PostsController _controller;



    public PostsControllerIntegrationTests()
    {
        // Set up in-memory database
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase("InMemoryBlogDb")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        _context = new BlogDbContext(options);
        _context.Database.EnsureCreated();

        // Create the repository and pass it to the controller
        _repository = new PostRepository(_context);
        _controller = new PostsController(_repository);

        // Reseed the database before each test
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        SeedDatabase();
    }

        // public PostsControllerIntegrationTests()
        // {
        //     // Set up in-memory database
        //     var serviceProvider = new ServiceCollection()
        //         .AddEntityFrameworkInMemoryDatabase()
        //         .BuildServiceProvider();

        //     var options = new DbContextOptionsBuilder<BlogDbContext>()
        //         .UseInMemoryDatabase("InMemoryBlogDb")
        //         .UseInternalServiceProvider(serviceProvider)
        //         .Options;

        //     _context = new BlogDbContext(options);
        //     _context.Database.EnsureCreated();

        //     // Create the repository and pass it to the controller
        //     _repository = new PostRepository(_context);
        //     _controller = new PostsController(_repository);

        //     // Seed the in-memory database with test data
        //     SeedDatabase();
        // }

        // private void SeedDatabase()
        // {
        //     var posts = new List<Post>
        //     {
        //         new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
        //         new Post { Id = 2, Title = "Post 2", Content = "Content 2" }
        //     };

        //     _context.Posts.AddRange(posts);
        //     _context.SaveChanges();
        // }
private void SeedDatabase()
{
    var posts = new List<Post>
    {
        new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
        new Post { Id = 2, Title = "Post 2", Content = "Content 2" }
    };

    _context.Posts.AddRange(posts);
    _context.SaveChanges();

    // Diagnostic: Check if the posts are added to the database
    var dbPostCount = _context.Posts.Count();
    Console.WriteLine($"Number of posts in the database after seeding: {dbPostCount}");
}


        [Fact]
        public async Task GetPosts_ReturnsOkResultWithPosts()
        {   

                var posts = new List<Post>
        {
            new Post { Id = 3, Title = "Post 1", Content = "Content 1" },
            new Post { Id = 4, Title = "Post 2", Content = "Content 2" }
        };

        _context.Posts.AddRange(posts);
        _context.SaveChanges();
            // Act
            var result = await _controller.GetPosts();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedPosts = okResult.Value.Should().BeAssignableTo<IEnumerable<Post>>().Subject;
            // returnedPosts.Should().HaveCount(2);
        }

        // [Fact]
        // public async Task GetPost_WithValidId_ReturnsOkResultWithPost()
        // {
        //     // Act
        //     var result = await _controller.GetPost(1);

        //     // Assert
        //     var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        //     var returnedPost = okResult.Value.Should().BeAssignableTo<Post>().Subject;
        //     returnedPost.Id.Should().Be(1);
        // }

        [Fact]
        public async Task CreatePost_AddsNewPost()
        {
            // Arrange
            var newPost = new Post { Id = 3, Title = "Post 3", Content = "Content 3" };

            // Act
            var result = await _controller.CreatePost(newPost);

            // Assert
            var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be(nameof(PostsController.GetPost));
            createdAtActionResult.RouteValues["id"].Should().Be(newPost.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(newPost);

            // Verify the post was added to the database
            var postInDb = await _context.Posts.FindAsync(newPost.Id);
            postInDb.Should().NotBeNull();
        }

        [Fact]
        public async Task DeletePost_RemovesPost()
        {
            // Act
            var result = await _controller.DeletePost(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            // Verify the post was removed from the database
            var postInDb = await _context.Posts.FindAsync(1);
            postInDb.Should().BeNull();
        }
    }
}
