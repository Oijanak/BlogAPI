using BlogApi.Application.DTOs;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Users.Commands.UpdateUserWithSpCommand;

public class UpdateUserWithSpCommandHandler:IRequestHandler<UpdateUserWithSpCommand, UserDTO>
{
    private readonly BlogDbContext _blogDbContext;

    public UpdateUserWithSpCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<UserDTO> Handle(UpdateUserWithSpCommand request, CancellationToken cancellationToken)
    {
        request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var users = await _blogDbContext.Users
            .FromSqlInterpolated($"EXEC spUpdateUser {request.UserId}, {request.Name}, {request.Email}, {request.Password}")
            .AsNoTracking()
            .ToListAsync();

        var updatedUser = users.FirstOrDefault();
        return new UserDTO(updatedUser);
    }
}