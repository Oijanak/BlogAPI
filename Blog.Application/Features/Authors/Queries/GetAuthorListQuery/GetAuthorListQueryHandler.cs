using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorListQuery;

public class GetAuthorListQueryHandler:IRequestHandler<GetAuthorListQuery,ApiResponse<IEnumerable<AuthorDto>>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetAuthorListQueryHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<IEnumerable<AuthorDto>>> Handle(GetAuthorListQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
       var result = await _blogDbContext.Authors
           .Select(author => new AuthorDto
           {
               AuthorId = author.AuthorId,
               AuthorName = author.AuthorName,
               AuthorEmail = author.AuthorEmail,
               Age = author.Age,
               CreatedBy = author.CreatedBy,
               isFollowed = userId!=null?author.Followers.Any(f => f.UserId == userId):null,
               FollowerCount=author.Followers.Count
           })
           .ToListAsync();
        return new ApiResponse<IEnumerable<AuthorDto>>
        {
            Data = result,
            Message = "Authors fectched successfully"
        };
    }
}