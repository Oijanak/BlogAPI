using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.SP.Blogs.Commands;

public class CreateBlogWithSpCommand:IRequest<ApiResponse<string>>
{
    public Guid AuthorId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
}