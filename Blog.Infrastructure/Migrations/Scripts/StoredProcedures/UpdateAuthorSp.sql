CREATE OR ALTER PROCEDURE spUpdateAuthor
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
END;