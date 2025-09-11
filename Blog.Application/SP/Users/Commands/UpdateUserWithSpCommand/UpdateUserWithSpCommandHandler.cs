using BlogApi.Application.DTOs;
using BlogApi.Application.Guards;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Users.Commands.UpdateUserWithSpCommand;

public class UpdateUserWithSpCommandHandler:IRequestHandler<UpdateUserWithSpCommand, ApiResponse<UserDTO>>
{
    private readonly IBlogDbContext _blogDbContext;

    public UpdateUserWithSpCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<UserDTO>> Handle(UpdateUserWithSpCommand request, CancellationToken cancellationToken)
    {
        UpdateUserWithSpGuard.ValidateWithGuard(request);
        request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var users = await _blogDbContext.Users
            .FromSqlInterpolated($"EXEC spUpdateUser {request.UserId}, {request.Name}, {request.Email}, {request.Password}")
            .AsNoTracking()
            .ToListAsync();

        var updatedUser = users.FirstOrDefault();
        return new ApiResponse<UserDTO>
        {
            Data = new UserDTO(updatedUser),
            Message = "User updated successfully",
        };
    }
}