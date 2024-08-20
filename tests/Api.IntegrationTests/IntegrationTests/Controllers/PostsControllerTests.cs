// using System.Collections.Generic;
// using System.Threading.Tasks;
// using API.Controllers;
// using Core.Entities;
// using Core.Interfaces;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using Xunit;

// namespace API.Tests.Controllers
// {
//     public class PostsControllerTests
//     {
//         private readonly Mock<IPostRepository> _mockRepository;
//         private readonly PostsController _controller;

//         public PostsControllerTests()
//         {
//             _mockRepository = new Mock<IPostRepository>();
//             _controller = new PostsController(_mockRepository.Object);
//         }

//         // [Fact]
//         // public async Task GetPosts_ReturnsOkResultWithPosts()
//         // {
//         //     // Arrange
//         //     var posts = new List<Post> { new Post(), new Post() };
//         //     _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(posts);

//         //     // Act
//         //     var result = await _controller.GetPosts();

//         //     // Assert
//         //     var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
//         //     var returnedPosts = okResult.Value.Should().BeAssignableTo<IEnumerable<Post>>().Subject;
//         //     returnedPosts.Should().BeEquivalentTo(posts);
//         // }

//         [Fact]
//         public async Task GetPost_WithValidId_ReturnsOkResultWithPost()
//         {
//             // Arrange
//             var postId = 1;
//             var post = new Post { Id = postId };
//             _mockRepository.Setup(repo => repo.GetByIdAsync(postId)).ReturnsAsync(post);

//             // Act
//             var result = await _controller.GetPost(postId);

//             // Assert
//             var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
//             var returnedPost = okResult.Value.Should().BeAssignableTo<Post>().Subject;
//             returnedPost.Should().BeEquivalentTo(post);
//         }

//         [Fact]
//         public async Task GetPost_WithInvalidId_ReturnsNotFound()
//         {
//             // Arrange
//             var postId = 1;
//             _mockRepository.Setup(repo => repo.GetByIdAsync(postId)).ReturnsAsync((Post)null);

//             // Act
//             var result = await _controller.GetPost(postId);

//             // Assert
//             result.Result.Should().BeOfType<NotFoundResult>();
//         }

//         [Fact]
//         public async Task CreatePost_ReturnsCreatedAtActionResult()
//         {
//             // Arrange
//             var post = new Post { Id = 1 };
//             _mockRepository.Setup(repo => repo.AddAsync(post)).Returns(Task.CompletedTask);

//             // Act
//             var result = await _controller.CreatePost(post);

//             // Assert
//             var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
//             createdAtActionResult.ActionName.Should().Be(nameof(PostsController.GetPost));
//             createdAtActionResult.RouteValues["id"].Should().Be(post.Id);
//             createdAtActionResult.Value.Should().BeEquivalentTo(post);
//         }

//         [Fact]
//         public async Task UpdatePost_WithValidIdAndPost_ReturnsNoContent()
//         {
//             // Arrange
//             var postId = 1;
//             var post = new Post { Id = postId };
//             _mockRepository.Setup(repo => repo.UpdateAsync(post)).Returns(Task.CompletedTask);

//             // Act
//             var result = await _controller.UpdatePost(postId, post);

//             // Assert
//             result.Should().BeOfType<NoContentResult>();
//         }

//         [Fact]
//         public async Task UpdatePost_WithMismatchedId_ReturnsBadRequest()
//         {
//             // Arrange
//             var postId = 1;
//             var post = new Post { Id = 2 };

//             // Act
//             var result = await _controller.UpdatePost(postId, post);

//             // Assert
//             result.Should().BeOfType<BadRequestResult>();
//         }

//         [Fact]
//         public async Task DeletePost_ReturnsNoContent()
//         {
//             // Arrange
//             var postId = 1;
//             _mockRepository.Setup(repo => repo.DeleteAsync(postId)).Returns(Task.CompletedTask);

//             // Act
//             var result = await _controller.DeletePost(postId);

//             // Assert
//             result.Should().BeOfType<NoContentResult>();
//         }
//     }
// }