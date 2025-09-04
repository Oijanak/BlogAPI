using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;

public class GetBlogListQueryHandler:IRequestHandler<GetBlogListQuery, IEnumerable<BlogDTO>>
{
    private readonly IBlogRepository _blogRepository;

    public GetBlogListQueryHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    
    public async Task<IEnumerable<BlogDTO>> Handle(GetBlogListQuery request, CancellationToken cancellationToken)
    {
        var blogs= await _blogRepository.GetAllAsync();
        return blogs.Select(blog => new BlogDTO()
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
        });
    }
}