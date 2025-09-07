using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;

public class GetBlogsByAuthorIdQuery:IRequest<IEnumerable<BlogDTO>>
{
    public Guid AuthorId { get; }

    public GetBlogsByAuthorIdQuery(Guid authorId)
    {
        AuthorId = authorId;
    }
    
    
}