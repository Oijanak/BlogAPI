using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;

public class CreateBlogCommand:IRequest<ApiResponse<BlogDTO>>
{
    public Guid AuthorId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public List<IFormFile>? Documents { get; set; } = new List<IFormFile>();

    public List<Guid> Categories { get; set; } = new List<Guid>();

}