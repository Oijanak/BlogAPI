using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spDeleteAuthor
    @AuthorId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Delete all blogs related to the author
        DELETE FROM Blogs
        WHERE AuthorId = @AuthorId;

        -- Delete the author
        DELETE FROM Authors
        WHERE AuthorId = @AuthorId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- re-throw the error
    END CATCH
END;
");
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spUpdateBlogWithAuthor
    @BlogId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(200),
    @BlogContent NVARCHAR(MAX),
    @AuthorId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Blogs
    SET 
        BlogTitle = @BlogTitle,
        BlogContent = @BlogContent,
        AuthorId = @AuthorId,
        UpdatedAt = GETUTCDATE()
    WHERE BlogId = @BlogId;
END
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spUpdateBlogWithAuthor");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spDeleteAuthor");

        }
    }
}
