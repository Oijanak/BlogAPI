CREATE OR ALTER PROCEDURE spCreateAuthor
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
END;