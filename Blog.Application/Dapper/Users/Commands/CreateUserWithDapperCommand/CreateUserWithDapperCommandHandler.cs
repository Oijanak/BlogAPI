using System.Data;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Users.Commands;

public class CreateUserWithDapperCommandHandler:IRequestHandler<CreateUserWithDapperCommand, ApiResponse<UserDTO>>
{
    private readonly IDbConnection _dbConnection;

    public CreateUserWithDapperCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<ApiResponse<UserDTO>> Handle(CreateUserWithDapperCommand request, CancellationToken cancellationToken)
    {
        var passwordHash=BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user= await _dbConnection.QueryFirstAsync<UserDTO>("spCreateUser",new {request.Name,request.Email,passwordHash},commandType:CommandType.StoredProcedure);
        return new ApiResponse<UserDTO>
        {
            Data = user,
            Message = "User created successfully"
        };
    }
}