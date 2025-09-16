using System.Data;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Authors.UpdateAuthorWithDapperCommand;

public class UpdateAuthorWithDapperCommandHandler:IRequestHandler<UpdateAuthorWithDapperCommand,ApiResponse<AuthorDto>>
{
    private readonly IDbConnection _dbConnection;

    public UpdateAuthorWithDapperCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(UpdateAuthorWithDapperCommand request, CancellationToken cancellationToken)
    {
        var author = await _dbConnection.QueryFirstAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.AuthorId });
        Guard.Against.Null(author,nameof(author),"Author with Id not found");
        var updatedAuthor= await _dbConnection.QueryFirstAsync<AuthorDto>("spUpdateAuthor",new{request.AuthorId,request.Author.AuthorEmail,request.Author.AuthorName,request.Author.Age},commandType:CommandType.StoredProcedure);
        return new ApiResponse<AuthorDto>
        {
            Data = updatedAuthor,
            Message = "Author created successfully"
        };
    }
}