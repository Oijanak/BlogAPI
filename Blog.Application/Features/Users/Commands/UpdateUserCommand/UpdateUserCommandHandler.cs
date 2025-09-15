using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

public class UpdateUserCommanHanler:IRequestHandler<UpdateUserCommand,ApiResponse<UserDTO>>
{
    private readonly IBlogDbContext _blogDbContext;
    public UpdateUserCommanHanler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _blogDbContext.Users.FindAsync(request.UserId);
        Guard.Against.Null(user,nameof(user),"User cannot be null");
        user.Name = request.User.Name ?? user.Name;
        user.Email = request.User.Email ?? user.Email;
        if (request.User.Password is not null)
            user.Password = request.User.Password;

        _blogDbContext.Users.Update(user);
        await _blogDbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<UserDTO>
        {
            Data = new UserDTO(user),
            Message = "User updated successfully",
        };
    }
    
    
    
}