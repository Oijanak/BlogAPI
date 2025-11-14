using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;

public class GetBlogsByAuthorIdQuery:IRequest<ApiResponse<IEnumerable<BlogPublicDto>>>
{
   
    public Guid AuthorId { get; set; }

    public int Page { get; set; } = 1;

    public int Limit { get; set; } = 10;

    
    
}