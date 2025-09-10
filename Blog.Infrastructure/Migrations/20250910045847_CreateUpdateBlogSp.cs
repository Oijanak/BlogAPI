using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUpdateBlogSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE spUpdateBlog
    @BlogId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(200),
    @BlogContent NVARCHAR(MAX),
    @AuthorId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Update the blog
    UPDATE Blogs
    SET 
        BlogTitle = @BlogTitle,
        BlogContent = @BlogContent,
        AuthorId = @AuthorId,
        UpdatedAt = GETUTCDATE()
    WHERE BlogId = @BlogId;

    -- Return the updated blog
    SELECT *
    FROM Blogs
    WHERE BlogId = @BlogId;
END;
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spUpdateBlog");
        }
    }
}
