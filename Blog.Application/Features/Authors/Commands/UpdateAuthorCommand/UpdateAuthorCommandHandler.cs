using System.Net;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.UpdateAuthorCommand;

public class UpdateAuthorCommandHandler:IRequestHandler<UpdateAuthorCommand, AuthorDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public UpdateAuthorCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<AuthorDTO> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        Author existingAuthor= await _blogDbContext.Authors.FindAsync(request.AuthorId) ??  throw new ApiException("Author not found with id "+request.AuthorId,HttpStatusCode.NotFound);
        existingAuthor.AuthorEmail = request.AuthorEmail;
        existingAuthor.AuthorName = request.AuthorName;
        existingAuthor.Age = request.Age;
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new AuthorDTO(existingAuthor);

    }
}