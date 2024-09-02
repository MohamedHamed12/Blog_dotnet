using API;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PostService : IPostService
{
    private readonly IRepository<Post> _repository;
    private readonly IMapper _mapper;

    public PostService(IRepository<Post> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
    {
        var posts = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PostDto>>(posts);
    }

    public async Task<PostDto?> GetPostByIdAsync(Guid id)
    {
        var post = await _repository.GetByIdAsync(id);
        return post == null ? null : _mapper.Map<PostDto>(post);
    }

    public async Task<Guid> CreatePostAsync(PostDto postDto)
    {
        var post = _mapper.Map<Post>(postDto);
        post.Id = Guid.NewGuid();
        await _repository.AddAsync(post);
        return post.Id;
    }

    public async Task UpdatePostAsync(Guid id, PostDto postDto)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null)
            throw new KeyNotFoundException("Post not found");

        _mapper.Map(postDto, post);
        await _repository.UpdateAsync(post);
    }

    public async Task DeletePostAsync(Guid id)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null)
            throw new KeyNotFoundException("Post not found");

        await _repository.DeleteAsync(post);
    }
}
