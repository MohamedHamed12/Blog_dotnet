using Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _CommentService;

    public CommentsController(ICommentService CommentService)
    {
        _CommentService = CommentService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<CommentDto>>> GetAll([FromQuery] SieveModel sieveModel)
    {
        var Comments = await _CommentService.GetAllCommentsAsync(sieveModel);
        return Ok(Comments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var Comment = await _CommentService.GetCommentByIdAsync(id);
        if (Comment == null)
            return NotFound();

        return Ok(Comment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommentDto CommentDto)
    {
        var id = await _CommentService.CreateCommentAsync(CommentDto);
        return CreatedAtAction(nameof(GetById), new { id }, CommentDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CommentDto CommentDto)
    {
        await _CommentService.UpdateCommentAsync(id, CommentDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _CommentService.DeleteCommentAsync(id);
        return NoContent();
    }
}
