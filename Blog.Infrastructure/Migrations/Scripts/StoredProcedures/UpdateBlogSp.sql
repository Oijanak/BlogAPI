CREATE OR ALTER PROCEDURE spUpdateBlog
    @BlogId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(200),
    @BlogContent NVARCHAR(MAX),
    @AuthorId UNIQUEIDENTIFIER,
    @UpdatedBy NVARCHAR(255)
    AS
BEGIN
    SET NOCOUNT ON;

UPDATE Blogs
SET
    BlogTitle = @BlogTitle,
    BlogContent = @BlogContent,
    AuthorId = @AuthorId,
    UpdatedAt = GETUTCDATE(),
    UpdatedBy=@UpdatedBy
WHERE BlogId = @BlogId;

SELECT *
FROM Blogs
WHERE BlogId = @BlogId;
END;