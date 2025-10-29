using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogReportQuery;

public class GetBlogReportExcelQueryHandler:IRequestHandler<GetBlogReportExcelQuery, byte[]>
{
    private  readonly IExcelService _excelService;
    private readonly IBlogDbContext _blogDbContext;

    public GetBlogReportExcelQueryHandler(IExcelService excelService, IBlogDbContext blogDbContext)
    {
        _excelService = excelService;
        _blogDbContext = blogDbContext;
    }
    public async Task<byte[]> Handle(GetBlogReportExcelQuery request, CancellationToken cancellationToken)
    {
        var blogReport = await _blogDbContext.Blogs
            .Select(b => new BlogReportDto()
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                AuthorName = b.Author.AuthorName,
                FavoritesCount = b.FavoritedBy.Count(),
                CommentsCount = b.Comments.Count()
            })
            .ToListAsync();

        var result =await  _excelService.GenerateBlogReportExcelAsync(blogReport);
        return result;
    }
}