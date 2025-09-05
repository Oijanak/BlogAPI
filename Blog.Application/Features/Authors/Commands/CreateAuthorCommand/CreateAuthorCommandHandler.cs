using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;

public class CreateAuthorCommandHandler:IRequestHandler<CreateAuthorCommand,AuthorDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public CreateAuthorCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<AuthorDTO> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        Author author=new Author
        {
            Age = request.Age,
            AuthorEmail = request.AuthorEmail,
            AuthorName = request.AuthorName,
        };
        await _blogDbContext.Authors.AddAsync(author, cancellationToken);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new AuthorDTO
        {
            AuthorId = author.AuthorId,
            Age = author.Age,
            AuthorEmail = author.AuthorEmail,
            AuthorName = author.AuthorName,
        };
    }
}