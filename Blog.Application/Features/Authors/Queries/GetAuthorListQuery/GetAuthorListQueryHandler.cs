using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorListQuery;

public class GetAuthorListQueryHandler:IRequestHandler<GetAuthorListQuery,IEnumerable<AuthorDTO>>
{
    private readonly BlogDbContext _blogDbContext;

    public GetAuthorListQueryHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<IEnumerable<AuthorDTO>> Handle(GetAuthorListQuery request, CancellationToken cancellationToken)
    {
        return await _blogDbContext.Authors.Select(author => new AuthorDTO
        {
            AuthorEmail = author.AuthorEmail,
            AuthorId = author.AuthorId,
            AuthorName = author.AuthorName,
            Age = author.Age
        }).ToListAsync();
    }
}