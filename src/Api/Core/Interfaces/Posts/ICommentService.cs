using Sieve.Models;

public interface ICommentService
{
    Task<PagedResult<CommentDto>> GetAllCommentsAsync(SieveModel sieveModel);
    Task<CommentDto?> GetCommentByIdAsync(Guid id);
    Task<Guid> CreateCommentAsync(CommentDto CommentDto);
    Task UpdateCommentAsync(Guid id, CommentDto CommentDto);
    Task DeleteCommentAsync(Guid id);
}
