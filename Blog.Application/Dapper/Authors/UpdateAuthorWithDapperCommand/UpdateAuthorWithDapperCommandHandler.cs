using System.Data;
using System.Security.Claims;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Authors.UpdateAuthorWithDapperCommand;

public class UpdateAuthorWithDapperCommandHandler:IRequestHandler<UpdateAuthorWithDapperCommand,ApiResponse<AuthorDto>>
{
    private readonly IDbConnection _dbConnection;
    private readonly string _currentUserId;
    public UpdateAuthorWithDapperCommandHandler(IDbConnection dbConnection,IHttpContextAccessor httpContextAccessor)
    {
        _dbConnection = dbConnection;
        _currentUserId=httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(UpdateAuthorWithDapperCommand request, CancellationToken cancellationToken)
    {
        var author = await _dbConnection.QueryFirstAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.AuthorId });
        Guard.Against.Null(author,nameof(author),"Author with Id not found");
        var updatedAuthor= await _dbConnection.QueryFirstAsync<AuthorDto>("spUpdateAuthor",new{request.AuthorId,request.Author.AuthorEmail,request.Author.AuthorName,request.Author.Age,UpdatedBy=_currentUserId},commandType:CommandType.StoredProcedure);
        return new ApiResponse<AuthorDto>
        {
            Data = updatedAuthor,
            Message = "Author created successfully"
        };
    }
}