using BlogBackend.Core.Specifications;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(int id);
        Task<IEnumerable<Post>>  GetAllAsync(PostSpecification spec);
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(int id);
    }
}
