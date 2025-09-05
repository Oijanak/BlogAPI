using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

public class UpdateUserCommanHanler:IRequestHandler<UpdateUserCommand,UserDTO>
{
    private readonly BlogDbContext _blogDbContext;
    public UpdateUserCommanHanler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _blogDbContext.Users.FindAsync(request.UserId)??
            throw new ApiException("User not found with id " + request.UserId, System.Net.HttpStatusCode.NotFound);;
        user.Name = request.Name ?? user.Name;
        user.Email = request.Email ?? user.Email;
        if (request.Password is not null)
            user.Password = request.Password;

        _blogDbContext.Users.Update(user);
        await _blogDbContext.SaveChangesAsync(cancellationToken);

        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email
        };
    }
}