using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Blogs.Commands.CreateBlogWithDapperCommand;

public class CreateBlogWithDapperCommand:IRequest<ApiResponse<BlogDTO>>
{
    public Guid AuthorId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public List<Guid>? Categories { get; set; }
    
    public List<IFormFile>? Files { get; set; }
}