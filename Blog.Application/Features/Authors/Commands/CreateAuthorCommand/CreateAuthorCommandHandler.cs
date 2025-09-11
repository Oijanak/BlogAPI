using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;

public class CreateAuthorCommandHandler:IRequestHandler<CreateAuthorCommand,ApiResponse<AuthorDto>>
{
    private readonly IBlogDbContext _blogDbContext;

    public CreateAuthorCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        Author author=new Author
        {
            Age = request.Age,
            AuthorEmail = request.AuthorEmail,
            AuthorName = request.AuthorName,
        };
        await _blogDbContext.Authors.AddAsync(author, cancellationToken);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<AuthorDto>
        {
            Data = new AuthorDto(author),
            Message = "Author created successfully",
        };
    }
}