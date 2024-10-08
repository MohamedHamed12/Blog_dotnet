using Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PostDto>>> GetAll([FromQuery] SieveModel sieveModel)
    {
        var posts = await _postService.GetAllPostsAsync(sieveModel);
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
            return NotFound();

        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostDto postDto)
    {
        var id = await _postService.CreatePostAsync(postDto);
        return CreatedAtAction(nameof(GetById), new { id }, postDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PostDto postDto)
    {
        await _postService.UpdatePostAsync(id, postDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _postService.DeletePostAsync(id);
        return NoContent();
    }
}
