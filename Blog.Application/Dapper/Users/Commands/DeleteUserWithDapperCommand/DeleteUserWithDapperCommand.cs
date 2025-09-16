using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Dapper.Users.Commands.DeleteUserWithDapperCommand;

public class DeleteUserWithDapperCommand: IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid UserId { get; set; }
}