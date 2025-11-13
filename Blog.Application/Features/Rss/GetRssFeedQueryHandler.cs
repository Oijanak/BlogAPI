using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Rss;

public class GetRssFeedQueryHandler : IRequestHandler<GetRssFeedQuery, string>
{
    private readonly IBlogDbContext _dbContext;

    public GetRssFeedQueryHandler(IBlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(GetRssFeedQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _dbContext.Blogs.Select(b => new {b.BlogId,b.BlogTitle,b.BlogContent,b.CreatedAt})
            .AsNoTracking()
            .OrderByDescending(b => b.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);

        var feed = new SyndicationFeed (
            "My Blog Feed",
            "Latest blog posts",
            new Uri("http://localhost:5058/api/blogs"),
            "BlogFeedId",
            DateTime.UtcNow
        );

        var items = blogs.Select(b => new SyndicationItem(
            b.BlogTitle,
            b.BlogContent.Length > 300 ? b.BlogContent[..300] + "..." : b.BlogContent,
            new Uri($"http://localhost:5058/api/blogs/{b.BlogId}"),
            b.BlogId.ToString(),
            b.CreatedAt
        )).ToList();

        feed.Items = items;
        
        using var stringWriter = new Utf8StringWriter();
        using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
        {
            var rssFormatter = new Rss20FeedFormatter(feed);
            rssFormatter.WriteTo(xmlWriter);
        }

        return stringWriter.ToString();
    }


    private class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}