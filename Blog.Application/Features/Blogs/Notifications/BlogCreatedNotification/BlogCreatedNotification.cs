using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Notifications.BlogCreatedNotification;

public class BlogCreatedNotification:INotification
{
    public Blog Blog { get; set; }
}