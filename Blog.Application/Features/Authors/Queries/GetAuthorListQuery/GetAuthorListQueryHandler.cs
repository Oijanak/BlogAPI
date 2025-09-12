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

    public GetAuthorListQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<IEnumerable<AuthorDto>>> Handle(GetAuthorListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<AuthorDto> result= await _blogDbContext.Authors.Select(author => new AuthorDto(author)).ToListAsync();
        return new ApiResponse<IEnumerable<AuthorDto>>
        {
            Data = result,
            Message = "Authors fecthed successfully"
        };
    }
}