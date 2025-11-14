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
        var userId = _currentUserService.UserId;
        var authorDto = await _blogDbContext.Authors
             .Where(a => a.AuthorId == request.AuthorId)
            .Select(a=>new AuthorDto { 
                AuthorId=a.AuthorId,
                AuthorName=a.AuthorName,
                AuthorEmail=a.AuthorEmail,
                isFollowed= userId != null ? a.Followers.Any(af => af.UserId == userId) : null,
                FollowerCount=a.Followers.Count

            })
            .FirstOrDefaultAsync();
       
        return new ApiResponse<AuthorDto>
        {
            
            Data = authorDto,
            Message = "Author fecthed successfully"
        };
    }
}