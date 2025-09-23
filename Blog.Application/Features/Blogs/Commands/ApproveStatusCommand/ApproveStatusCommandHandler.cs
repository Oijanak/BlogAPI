using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Enum;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.ApproveStatusCommand;

public class ApproveStatusCommandHandler:IRequestHandler<ApproveStatusCommand, ApiResponse<string>>
{
    private readonly IBlogDbContext  _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public ApproveStatusCommandHandler(IBlogDbContext context,ICurrentUserService currentUserService)
    {
        _blogDbContext = context;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<string>> Handle(ApproveStatusCommand request, CancellationToken cancellationToken)
    {
        Blog existingBlog = await _blogDbContext.Blogs.FindAsync(request.BlogId) ;
        Guard.Against.Null(existingBlog,nameof(existingBlog));
        existingBlog.ApproveStatus = ApproveStatus.Approved;
        existingBlog.ApprovedBy=_currentUserService.UserId;
        _blogDbContext.Blogs.Update(existingBlog);
        await _blogDbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<string>
        {
            Message = $"Blog '{existingBlog.BlogTitle}' has been approved successfully."
        };
    }
}