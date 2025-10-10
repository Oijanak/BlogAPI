using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogDocumentQuery;

public class GetBlogDocumentQuery:IRequest<FileStreamResult>
{
    [FromRoute]
    public Guid BlogDocumentId { get; set; }
}