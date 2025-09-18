using System.Data;
using System.Security.Claims;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Authors.CreateAuthorWithDapperCommand;

public class CreateAuthorWithDapperCommandHandler:IRequestHandler<CreateAuthorWithDapperCommand, ApiResponse<AuthorDto>>
{
    private readonly IDbConnection _dbConnection;
    private readonly string _currentUserId;

    public CreateAuthorWithDapperCommandHandler(IDbConnection dbConnection,IHttpContextAccessor httpContextAccessor)
    {
        _dbConnection = dbConnection;
        _currentUserId=httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(CreateAuthorWithDapperCommand request, CancellationToken cancellationToken)
    {
        var author= await _dbConnection.QueryFirstAsync<AuthorDto>("spCreateAuthor",new{request.AuthorEmail,request.AuthorName,request.Age,CreatedBy=_currentUserId},commandType:CommandType.StoredProcedure);
        return new ApiResponse<AuthorDto>
        {
            Data = author,
            Message = "Author created successfully"
        };
    }
}