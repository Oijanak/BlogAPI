using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;

public class CreateBlogCommandHandler:IRequestHandler<CreateBlogCommand,BlogDTO>
{
    private readonly IBlogRepository _blogRepository;

    public CreateBlogCommandHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    public async Task<BlogDTO> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        Blog blog = new Blog
        {
            BlogTitle = request.BlogTitle,
            BlogContent = request.BlogContent,
            UserId = request.UserId
        };
       await _blogRepository.AddAsync(blog);
        return new BlogDTO()
        {
            BlogId = blog.BlogId,
            BlogTitle = blog.BlogTitle,
            BlogContent = blog.BlogContent,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt
        };
        
    }
}