using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Users.Commands;

public class CreateUserWithSpCommandHandler:IRequestHandler<CreateUserWithSpCommand, ApiResponse<UserDTO>>
{
    private readonly IBlogDbContext _blogDbContext;

    public CreateUserWithSpCommandHandler(IBlogDbContext blogDbcontext)
    {
        _blogDbContext = blogDbcontext;
    }

    public async Task<ApiResponse<UserDTO>> Handle(CreateUserWithSpCommand request, CancellationToken cancellationToken)
    {
        request.Password=BCrypt.Net.BCrypt.HashPassword(request.Password);
        var users = await _blogDbContext.Users
            .FromSqlInterpolated($"EXEC spCreateUser {request.Name}, {request.Email}, {request.Password}")
            .AsNoTracking()
            .ToListAsync(); 

        var user = users.FirstOrDefault();
        return new ApiResponse<UserDTO>
        {
            Data = new UserDTO(user),
            Message = "User created successfully",
        };
    }
}