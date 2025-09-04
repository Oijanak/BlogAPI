using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQueryHandler:IRequestHandler<GetBlogQuery,BlogDTO>
{
    private readonly IBlogRepository _blogRepository;

    public GetBlogQueryHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    
    public async Task<BlogDTO> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        Blog blog = await _blogRepository.GetByIdAsync(request.BlogId) ?? throw new Exception("Blog not found");
        return new BlogDTO()
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
        };
    }
}