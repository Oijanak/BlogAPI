using BlogApi.Application.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Blogs.Notifications.BlogCreatedNotification;

public class BlogCreatedNotificationHandler:INotificationHandler<BlogCreatedNotification>
{
    private readonly IEmailService _emailService;
    private readonly IBlogDbContext _dbContext;

    public BlogCreatedNotificationHandler(IEmailService emailService, IBlogDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(BlogCreatedNotification notification, CancellationToken cancellationToken)
    {
        var blog = notification.Blog;
        var followerEmails = await _dbContext.AuthorFollowers
            .Where(f => f.AuthorId == blog.AuthorId)
            .Include(f=>f.User)
            .Select(f => f.User)
            .ToListAsync();
        string template = $@"
<p>Hello @Name,</p>
<p>A new blog has been published: <strong>{blog.BlogTitle}</strong></p>
<p>
    You can read it here: 
    <a href='http://localhost:5058/blogs/{blog.BlogId}' target='_blank'>
        View Blog
    </a>
</p>
<p>Best regards,<br/>BlogApp Team</p>";
        foreach (var user in followerEmails)
        {
            BackgroundJob.Enqueue<IEmailService>(x =>
                x.SendEmailAsync(user.Email, $"{user.Name}", $"Check out the new blog: {blog.BlogTitle} by {blog.Author.AuthorName}",template));
        }
    }
}