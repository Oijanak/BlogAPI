using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.UpdateAuthorCommand;

public class UpdateAuthorCommandHandler:IRequestHandler<UpdateAuthorCommand, ApiResponse<AuthorDto>>
{
    private readonly IBlogDbContext _blogDbContext;

    public UpdateAuthorCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        Author existingAuthor= await _blogDbContext.Authors.FindAsync(request.AuthorId) ;
        ArgumentNullException.ThrowIfNull(existingAuthor,nameof(existingAuthor));
        existingAuthor.AuthorEmail = request.AuthorEmail;
        existingAuthor.AuthorName = request.AuthorName;
        existingAuthor.Age = request.Age;
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<AuthorDto>
        {
            Data = new AuthorDto(existingAuthor),
            Message = "Autho updated successfully"
        };

    }
}