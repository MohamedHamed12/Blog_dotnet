using API;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _repository;
    private readonly IMapper _mapper;
    private readonly SieveProcessor _sieveProcessor;

    private readonly BlogDbContext _context;

    public CommentService(
        IRepository<Comment> repository,
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
    public async Task<PagedResult<CommentDto>> GetAllCommentsAsync(SieveModel sieveModel)
    {
        var CommentsQuery = _context.Comments.AsNoTracking();
        var totalCount = await CommentsQuery.CountAsync();

        // Apply Sieve filters, sorting, and pagination
        CommentsQuery = _sieveProcessor.Apply(sieveModel, CommentsQuery, applyPagination: false);

        // Pagination
        var pagedComments = await _sieveProcessor
            .Apply(sieveModel, CommentsQuery, applyPagination: true)
            .ToListAsync();

        var CommentDtos = _mapper.Map<IEnumerable<CommentDto>>(pagedComments);

        return new PagedResult<CommentDto>
        {
            Data = CommentDtos,
            TotalCount = totalCount,
            PageNumber = sieveModel?.Page ?? 1,
            PageSize = sieveModel?.PageSize ?? totalCount,
        };
    }

    public async Task<CommentDto?> GetCommentByIdAsync(Guid id)
    {
        var Comment = await _repository.GetByIdAsync(id);
        return Comment == null ? null : _mapper.Map<CommentDto>(Comment);
    }

    public async Task<Guid> CreateCommentAsync(CommentDto CommentDto)
    {
        var Comment = _mapper.Map<Comment>(CommentDto);
        Comment.Id = Guid.NewGuid();
        await _repository.AddAsync(Comment);
        return Comment.Id;
    }

    public async Task UpdateCommentAsync(Guid id, CommentDto CommentDto)
    {
        var Comment = await _repository.GetByIdAsync(id);
        if (Comment == null)
            throw new KeyNotFoundException("Comment not found");

        _mapper.Map(CommentDto, Comment);
        await _repository.UpdateAsync(Comment);
    }

    public async Task DeleteCommentAsync(Guid id)
    {
        var Comment = await _repository.GetByIdAsync(id);
        if (Comment == null)
            throw new KeyNotFoundException("Comment not found");

        await _repository.DeleteAsync(Comment);
    }
}
