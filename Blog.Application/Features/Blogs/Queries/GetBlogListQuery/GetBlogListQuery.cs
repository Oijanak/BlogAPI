using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;

public class GetBlogListQuery: IRequest<IEnumerable<BlogDTO>>
{
    
}