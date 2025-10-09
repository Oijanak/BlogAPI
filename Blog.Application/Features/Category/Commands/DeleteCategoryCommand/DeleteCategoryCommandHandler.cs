using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;

namespace BlogApi.Application.Features.Category.Commands;

public class DeleteCategoryCommandHandler:IRequestHandler<DeleteCategoryCommand,ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteCategoryCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
       var exitingCategory = await _blogDbContext.Categories.FindAsync(request.CategoryId);
       _blogDbContext.Categories.Remove(exitingCategory);
       await _blogDbContext.SaveChangesAsync(cancellationToken);
       return new ApiResponse<string>
       {
           Message = "Category deleted",
       };
    }
}