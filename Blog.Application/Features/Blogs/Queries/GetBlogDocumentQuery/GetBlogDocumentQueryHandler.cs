using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogDocumentQuery;

public class GetBlogDocumentQueryHandler:IRequestHandler<GetBlogDocumentQuery,FileStreamResult>
{
    private readonly IFileService _fileService;
    
    public GetBlogDocumentQueryHandler(IFileService fileService)
    {
        _fileService = fileService;
    }
    public async Task<FileStreamResult> Handle(GetBlogDocumentQuery request, CancellationToken cancellationToken)
    {
        var fileResult = await _fileService.GetFileByIdAsync(request.BlogDocumentId);
        return fileResult;
    }
}