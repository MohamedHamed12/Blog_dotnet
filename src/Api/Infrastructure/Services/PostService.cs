using API;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

public class PostService : IPostService
{
    private readonly IRepository<Post> _repository;
    private readonly IMapper _mapper;
    private readonly SieveProcessor _sieveProcessor;

    private readonly BlogDbContext _context;

    public PostService(
        IRepository<Post> repository,
        IMapper mapper,
        SieveProcessor sieveProcessor,
        BlogDbContext context
    )
    {
        _repository = repository;
        _mapper = mapper;
        _sieveProcessor = sieveProcessor;
        _context = context;
    }
public async Task<PagedResult<PostDto>> GetAllPostsAsync(SieveModel sieveModel)

    // public async Task<PagedResult<PostDto>> GetAllPostsAsync(SieveModel sieveModel)
    {
        var postsQuery = _context.Posts.AsNoTracking();
        var totalCount = await postsQuery.CountAsync();

        // Apply Sieve filters, sorting, and pagination
        postsQuery = _sieveProcessor.Apply(sieveModel, postsQuery, applyPagination: false);

        // Pagination
        var pagedPosts = await _sieveProcessor
            .Apply(sieveModel, postsQuery, applyPagination: true)
            .ToListAsync();

        var postDtos = _mapper.Map<IEnumerable<PostDto>>(pagedPosts);

        return new PagedResult<PostDto>
        {
            Data = postDtos,
            TotalCount = totalCount,
            PageNumber = sieveModel?.Page ?? 1,
            PageSize = sieveModel?.PageSize ?? totalCount,
        };
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
