using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SpUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spUpdateUser
    @UserId UNIQUEIDENTIFIER,
    @Name NVARCHAR(MAX),
    @Email NVARCHAR(MAX),
    @PasswordHash NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [Users]
    SET 
        Name = @Name,
        Email = @Email,
        PasswordHash = @PasswordHash
    OUTPUT inserted.*
    WHERE UserId = @UserId;
END

");
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spDeleteUser
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Users]
    WHERE UserId = @UserId;
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spUpdateUser");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spDeleteUser");
        }
    }
}
