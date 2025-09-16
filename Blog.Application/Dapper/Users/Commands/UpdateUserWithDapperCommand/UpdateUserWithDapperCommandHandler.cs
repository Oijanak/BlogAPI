using System.Data;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Users.Commands.UpdateUserWithDapperCommand;

public class UpdateUserWithDapperCommandHandler:IRequestHandler<UpdateUserWithDapperCommand,ApiResponse<UserDTO>>
{
    private readonly IDbConnection _dbConnection;

    public UpdateUserWithDapperCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<ApiResponse<UserDTO>> Handle(UpdateUserWithDapperCommand request, CancellationToken cancellationToken)
    {
        var existingUser=await _dbConnection.QueryFirstOrDefaultAsync<UserDTO>("select * from [Users] where UserId=@UserId",new{UserId = request.UserId});
        Guard.Against.Null(existingUser, nameof(existingUser),"User with Id is not found");
        var passwordHash=BCrypt.Net.BCrypt.HashPassword(request.User.Password); 
       var user=await _dbConnection.QueryFirstAsync<UserDTO>("spUpdateUser",new {request.UserId,request.User.Name,request.User.Email,passwordHash},commandType:CommandType.StoredProcedure);
       return new ApiResponse<UserDTO>
       {
           Data = user,
           Message = "User updated successfully"
       };
    }
}