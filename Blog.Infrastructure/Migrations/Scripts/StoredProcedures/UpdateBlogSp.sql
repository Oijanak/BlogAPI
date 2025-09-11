CREATE OR ALTER PROCEDURE spUpdateBlog
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

SELECT *
FROM Blogs
WHERE BlogId = @BlogId;
END;