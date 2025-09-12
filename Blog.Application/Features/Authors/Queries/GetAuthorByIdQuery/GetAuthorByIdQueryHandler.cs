using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;

public class GetAuthorByIdQueryHandler:IRequestHandler<GetAuthorByIdQuery,ApiResponse<AuthorDto>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetAuthorByIdQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        Author author= await _blogDbContext.Authors.FindAsync(request.AuthorId) ;
        Guard.Against.Null(author,nameof(author),"Author cannot be null");
        return new ApiResponse<AuthorDto>
        {
            Data = new AuthorDto(author),
            Message = "Author fecthed successfully"
        };
    }
}