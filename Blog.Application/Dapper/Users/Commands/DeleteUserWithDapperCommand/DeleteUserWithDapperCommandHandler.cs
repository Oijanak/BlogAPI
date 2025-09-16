using System.Data;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Users.Commands.DeleteUserWithDapperCommand;

public class DeleteUserWithDapperCommandHandler:IRequestHandler<DeleteUserWithDapperCommand, ApiResponse<string>>
{
    private readonly IDbConnection _dbConnection;

    public DeleteUserWithDapperCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ApiResponse<string>> Handle(DeleteUserWithDapperCommand request, CancellationToken cancellationToken)
    {
        var existingUser=await _dbConnection.QueryFirstOrDefaultAsync<UserDTO>("select * from [Users] where UserId=@UserId",new{UserId = request.UserId});
        Guard.Against.Null(existingUser, nameof(existingUser),"User with Id is not found");
        await _dbConnection.ExecuteAsync(
            "spDeleteUser",
            request,
            commandType: CommandType.StoredProcedure);

        return new ApiResponse<string>
        {
            Message = "User deleted successfully"
        };
    }
}