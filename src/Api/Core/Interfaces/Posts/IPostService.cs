using Sieve.Models;

public interface IPostService
{
    Task<PagedResult<PostDto>> GetAllPostsAsync(SieveModel sieveModel);
    Task<PostDto?> GetPostByIdAsync(Guid id);
    Task<Guid> CreatePostAsync(PostDto postDto);
    Task UpdatePostAsync(Guid id, PostDto postDto);
    Task DeletePostAsync(Guid id);
}
