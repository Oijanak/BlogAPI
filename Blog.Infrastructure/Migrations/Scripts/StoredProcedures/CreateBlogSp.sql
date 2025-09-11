CREATE OR ALTER PROCEDURE spCreateBlogWithAuthor
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
END;