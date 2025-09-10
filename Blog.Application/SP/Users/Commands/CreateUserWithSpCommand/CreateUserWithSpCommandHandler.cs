using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Users.Commands;

public class CreateUserWithSpCommandHandler:IRequestHandler<CreateUserWithSpCommand, UserDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public CreateUserWithSpCommandHandler(BlogDbContext blogDbcontext)
    {
        _blogDbContext = blogDbcontext;
    }

    public async Task<UserDTO> Handle(CreateUserWithSpCommand request, CancellationToken cancellationToken)
    {
        request.Password=BCrypt.Net.BCrypt.HashPassword(request.Password);
        var users = await _blogDbContext.Users
            .FromSqlInterpolated($"EXEC spCreateUser {request.Name}, {request.Email}, {request.Password}")
            .AsNoTracking()
            .ToListAsync(); 

        var user = users.FirstOrDefault();
        return new UserDTO(user);
    }
}