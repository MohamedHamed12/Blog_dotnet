public interface IPostService
{
    Task<IEnumerable<PostDto>> GetAllPostsAsync();
    Task<PostDto?> GetPostByIdAsync(Guid id);
    Task<Guid> CreatePostAsync(PostDto postDto);
    Task UpdatePostAsync(Guid id, PostDto postDto);
    Task DeletePostAsync(Guid id);
}
