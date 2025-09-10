using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeBlogUpdatesp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spUpdateBlogWithAuthor
    @BlogId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(200),
    @BlogContent NVARCHAR(MAX),
    @AuthorId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the blog exists
    IF NOT EXISTS (SELECT 1 FROM Blogs WHERE BlogId = @BlogId)
    BEGIN
        RAISERROR('Blog not found.', 16, 1);
        RETURN;
    END

    -- Check if the author exists
    IF NOT EXISTS (SELECT 1 FROM Authors WHERE AuthorId = @AuthorId)
    BEGIN
        RAISERROR('Author not found.', 16, 1);
        RETURN;
    END

    -- Perform the update
    UPDATE Blogs
    SET 
        BlogTitle = @BlogTitle,
        BlogContent = @BlogContent,
        AuthorId = @AuthorId,
        UpdatedAt = GETUTCDATE()
    WHERE BlogId = @BlogId;

    -- Return the updated row
    SELECT *
    FROM Blogs
    WHERE BlogId = @BlogId;
END

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
