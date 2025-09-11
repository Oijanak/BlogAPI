using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<UserDTO>>
{
    private readonly IBlogDbContext _blogDbContext;
    public CreateUserCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<ApiResponse<UserDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
        await _blogDbContext.Users.AddAsync(user);
        await _blogDbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<UserDTO>
        {
            Data = new UserDTO(user),
            Message = "User created successfully"
        };
    }
}