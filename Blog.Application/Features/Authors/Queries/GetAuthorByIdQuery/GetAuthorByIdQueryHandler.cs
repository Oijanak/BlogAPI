using System.Net;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;

public class GetAuthorByIdQueryHandler:IRequestHandler<GetAuthorByIdQuery,AuthorDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public GetAuthorByIdQueryHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<AuthorDTO> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        Author author= await _blogDbContext.Authors.FindAsync(request.AuthorId) ??  throw new ApiException("Author not found with id "+request.AuthorId,HttpStatusCode.NotFound);
        return new AuthorDTO(author);
    }
}