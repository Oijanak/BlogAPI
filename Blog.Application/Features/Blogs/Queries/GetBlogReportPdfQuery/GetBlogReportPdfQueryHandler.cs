using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogReportPdfQuery;

public class GetBlogReportPdfQueryHandler:IRequestHandler<GetBlogReportPdfQuery,byte[]>
{
    private readonly IPdfService _pdfService;
    private readonly IBlogDbContext _blogDbContext;
    public GetBlogReportPdfQueryHandler(IPdfService pdfService, IBlogDbContext blogDbContext)
    {
        _pdfService = pdfService;
        _blogDbContext = blogDbContext;
    }
    public async Task<byte[]> Handle(GetBlogReportPdfQuery request, CancellationToken cancellationToken)
    {
        var blogReport = await _blogDbContext.Blogs.AsNoTracking()
            .Select(b => new BlogReportDto()
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                AuthorName = b.Author.AuthorName,
                FavoritesCount = b.FavoritedBy.Count(),
                CommentsCount = b.Comments.Count()
            })
            .ToListAsync();

        var result =await  _pdfService.GenerateBlogReportPdfAsync(blogReport);
        return result;
    }
}