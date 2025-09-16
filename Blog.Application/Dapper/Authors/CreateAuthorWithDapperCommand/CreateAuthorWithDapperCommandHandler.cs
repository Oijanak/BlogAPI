using System.Data;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Authors.CreateAuthorWithDapperCommand;

public class CreateAuthorWithDapperCommandHandler:IRequestHandler<CreateAuthorWithDapperCommand, ApiResponse<AuthorDto>>
{
    private readonly IDbConnection _dbConnection;

    public CreateAuthorWithDapperCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<ApiResponse<AuthorDto>> Handle(CreateAuthorWithDapperCommand request, CancellationToken cancellationToken)
    {
        var author= await _dbConnection.QueryFirstAsync<AuthorDto>("spCreateAuthor",request,commandType:CommandType.StoredProcedure);
        return new ApiResponse<AuthorDto>
        {
            Data = author,
            Message = "Author created successfully"
        };
    }
}