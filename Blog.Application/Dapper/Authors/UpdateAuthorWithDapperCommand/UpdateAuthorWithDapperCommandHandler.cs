using System.Data;
using System.Security.Claims;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Authors.UpdateAuthorWithDapperCommand;

public class UpdateAuthorWithDapperCommandHandler:IRequestHandler<UpdateAuthorWithDapperCommand,ApiResponse<AuthorDto>>
{
    private readonly IDbConnection _dbConnection;
    private readonly ICurrentUserService _currentUserService;
    public UpdateAuthorWithDapperCommandHandler(IDbConnection dbConnection,ICurrentUserService currentUserService)
    {
        _dbConnection = dbConnection;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(UpdateAuthorWithDapperCommand request, CancellationToken cancellationToken)
    {
        var currentUserId=_currentUserService.UserId;
        var author = await _dbConnection.QueryFirstAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.AuthorId });
        Guard.Against.Null(author,nameof(author),"Author with Id not found");
        var updatedAuthor= await _dbConnection.QueryFirstAsync<AuthorDto>("spUpdateAuthor",new{request.AuthorId,request.Author.AuthorEmail,request.Author.AuthorName,request.Author.Age,UpdatedBy=currentUserId},commandType:CommandType.StoredProcedure);
        return new ApiResponse<AuthorDto>
        {
            Data = updatedAuthor,
            Message = "Author created successfully"
        };
    }
}