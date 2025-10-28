using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;

public class GetAuthorByIdQueryHandler:IRequestHandler<GetAuthorByIdQuery,ApiResponse<AuthorDto>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetAuthorByIdQueryHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;

    }
    public async Task<ApiResponse<AuthorDto>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _blogDbContext.Authors
            .Include(a => a.Followers)
            .FirstOrDefaultAsync(a => a.AuthorId == request.AuthorId);
        Guard.Against.Null(author,nameof(author),"Author cannot be null");
        var authorDto = new AuthorDto(author);
        var userId = _currentUserService.UserId;
        authorDto.isFollowed =userId!=null? author.Followers.Any(af => af.UserId == userId):null;
        return new ApiResponse<AuthorDto>
        {
            
            Data = authorDto,
            Message = "Author fecthed successfully"
        };
    }
}