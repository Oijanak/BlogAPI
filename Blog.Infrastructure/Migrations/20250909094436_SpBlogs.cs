using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SpBlogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spCreateBlogWithAuthor
    @AuthorId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(200),
    @BlogContent NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewBlogId UNIQUEIDENTIFIER = NEWID();

    INSERT INTO [Blogs] (BlogId, BlogTitle, BlogContent, AuthorId, CreatedAt, UpdatedAt)
    OUTPUT inserted.*
    VALUES (@NewBlogId, @BlogTitle, @BlogContent, @AuthorId, GETUTCDATE(), GETUTCDATE());

    SELECT 
        b.BlogId,
        b.BlogTitle,
        b.BlogContent,
        b.CreatedAt,
        b.UpdatedAt,
        a.AuthorId,
        a.AuthorName,
        a.AuthorEmail,
        a.Age
    FROM Blogs b
    INNER JOIN Authors a ON b.AuthorId = a.AuthorId
    WHERE b.BlogId = @NewBlogId;
END

");

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spUpdateBlogWithAuthor
    @BlogId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(200),
    @BlogContent NVARCHAR(MAX),
    @AuthorId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Update the blog with new title, content, and author
    UPDATE [Blogs]
    SET 
        BlogTitle = @BlogTitle,
        BlogContent = @BlogContent,
        AuthorId = @AuthorId,
        UpdatedAt = GETUTCDATE()
    OUTPUT inserted.*
    WHERE BlogId = @BlogId;

   
    SELECT 
        b.BlogId,
        b.BlogTitle,
        b.BlogContent,
        b.CreatedAt,
        b.UpdatedAt,
        a.AuthorId,
        a.AuthorName,
        a.AuthorEmail,
        a.Age
    FROM Blogs b
    INNER JOIN Authors a ON b.AuthorId = a.AuthorId
    WHERE b.BlogId = @BlogId;
END
");
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spDeleteBlog
    @BlogId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Blogs
    WHERE BlogId = @BlogId;
END
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spCreateBlogWithAuthor");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spUpdateBlogWithAuthor");
        }
    }
}
