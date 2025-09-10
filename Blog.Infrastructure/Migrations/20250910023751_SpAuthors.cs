using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SpAuthors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spCreateAuthor
    @AuthorEmail NVARCHAR(255),
    @AuthorName NVARCHAR(255),
    @Age INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AuthorId UNIQUEIDENTIFIER = NEWID();

    INSERT INTO Authors (AuthorId, Age, AuthorEmail, AuthorName)
    OUTPUT inserted.*
    VALUES (@AuthorId, @Age, @AuthorEmail, @AuthorName);
END");

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spUpdateAuthor
    @AuthorId UNIQUEIDENTIFIER,
    @AuthorEmail NVARCHAR(255),
    @AuthorName NVARCHAR(255),
    @Age INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Authors
    SET 
        Age = @Age,
        AuthorEmail = @AuthorEmail,
        AuthorName = @AuthorName
    OUTPUT inserted.*
    WHERE AuthorId = @AuthorId;
END;");


            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spDeleteAuthor
    @AuthorId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Authors
    WHERE AuthorId = @AuthorId;
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spCreateAuthor");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spUpdateAuthor");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spDeleteAuthor");

        }
    }
}
