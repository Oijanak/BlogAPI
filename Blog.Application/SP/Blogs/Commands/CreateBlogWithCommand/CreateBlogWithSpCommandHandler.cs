using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Blogs.Commands;

public class CreateBlogWithSpCommandHandler:IRequestHandler<CreateBlogWithSpCommand,ApiResponse<BlogDTO>>
{
    private readonly IBlogDbContext _blogDbContext;

    public CreateBlogWithSpCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(CreateBlogWithSpCommand request, CancellationToken cancellationToken)
    {
        var blogs = await _blogDbContext.Blogs
            .FromSqlInterpolated($"EXEC spCreateBlogWithAuthor {request.AuthorId}, {request.BlogTitle}, {request.BlogContent}")
            .AsNoTracking()
            .ToListAsync();
        ArgumentNullException.ThrowIfNull(blogs,nameof(blogs));
        var result=blogs.FirstOrDefault();
        var blogDtos=new BlogDTO
        {
            BlogId = result.BlogId,
            BlogTitle = result.BlogTitle,
            BlogContent = result.BlogContent,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
        };
        return new ApiResponse<BlogDTO>
        {
            Data = blogDtos,
            Message = "Blog created successfully"
        };

    }
}
public class BlogWithAuthorDTO
{
    public Guid BlogId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public string AuthorEmail { get; set; }
    public int Age { get; set; }
}