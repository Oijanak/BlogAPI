using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;

public class GetBlogsByAuthorIdQuery:IRequest<ApiResponse<IEnumerable<BlogDTO>>>
{
    [FromRoute]
    public Guid AuthorId { get; }

    public GetBlogsByAuthorIdQuery(Guid authorId)
    {
        AuthorId = authorId;
    }
    
    
}