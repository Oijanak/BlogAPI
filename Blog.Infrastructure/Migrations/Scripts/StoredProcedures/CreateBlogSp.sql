CREATE OR ALTER PROCEDURE spCreateBlogWithAuthor
    @AuthorId UNIQUEIDENTIFIER,
    @BlogTitle NVARCHAR(200),
    @BlogContent NVARCHAR(MAX),
    @CreatedBy NVARCHAR(255)
    AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewBlogId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [Blogs] (BlogId, BlogTitle, BlogContent, AuthorId, CreatedAt, UpdatedAt,CreatedBy)
VALUES (@NewBlogId, @BlogTitle, @BlogContent, @AuthorId, GETUTCDATE(), GETUTCDATE(),@CreatedBy);

SELECT
    b.BlogId,
    b.BlogTitle,
    b.BlogContent,
    b.CreatedAt,
    b.UpdatedAt,
    b.CreatedBy,
    a.AuthorId,
    a.AuthorName,
    a.AuthorEmail,
    a.Age,
    a.CreatedBy
FROM Blogs b
         INNER JOIN Authors a ON b.AuthorId = a.AuthorId
WHERE b.BlogId = @NewBlogId;
END;