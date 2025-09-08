using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorsWithAgeQuery;

public class GetAuthorsWithAgeQueryHandler:IRequestHandler<GetAuthorsWithAgeQuery,IEnumerable<AuthorDTO>>
{
    private readonly BlogDbContext  _blogDbContext;

    public GetAuthorsWithAgeQueryHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<IEnumerable<AuthorDTO>> Handle(GetAuthorsWithAgeQuery request, CancellationToken cancellationToken)
    {
        return  _blogDbContext.Authors
            .FromSqlInterpolated($"EXEC spGetAuthorsWithAgeBetween {request.Age1}, {request.Age2}")
            .AsEnumerable()
            .Select(author => new AuthorDTO(author));
    }
}