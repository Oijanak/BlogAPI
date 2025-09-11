using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorsWithAgeQuery;

public class GetAuthorsWithAgeQueryHandler:IRequestHandler<GetAuthorsWithAgeQuery,ApiResponse<IEnumerable<AuthorDto>>>
{
    private readonly IBlogDbContext  _blogDbContext;

    public GetAuthorsWithAgeQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<IEnumerable<AuthorDto>>> Handle(GetAuthorsWithAgeQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<AuthorDto> result= _blogDbContext.Authors
            .FromSqlInterpolated($"EXEC spGetAuthorsWithAgeBetween {request.Age1}, {request.Age2}")
            .AsEnumerable()
            .Select(author => new AuthorDto(author));
        return new ApiResponse<IEnumerable<AuthorDto>>
        {
            Data = result,
            Message = "Authors fetched successfully"
        };
    }
}