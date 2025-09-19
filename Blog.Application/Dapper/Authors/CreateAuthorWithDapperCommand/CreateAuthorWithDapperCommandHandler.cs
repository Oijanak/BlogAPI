using System.Data;
using System.Security.Claims;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Authors.CreateAuthorWithDapperCommand;

public class CreateAuthorWithDapperCommandHandler:IRequestHandler<CreateAuthorWithDapperCommand, ApiResponse<AuthorDto>>
{
    private readonly IDbConnection _dbConnection;
    private readonly ICurrentUserService _currentUserService;

    public CreateAuthorWithDapperCommandHandler(IDbConnection dbConnection,ICurrentUserService currentUserService)
    {
        _dbConnection = dbConnection;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(CreateAuthorWithDapperCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        var author= await _dbConnection.QueryFirstAsync<AuthorDto>("spCreateAuthor",new{request.AuthorEmail,request.AuthorName,request.Age,CreatedBy=currentUserId},commandType:CommandType.StoredProcedure);
        return new ApiResponse<AuthorDto>
        {
            Data = author,
            Message = "Author created successfully"
        };
    }
}