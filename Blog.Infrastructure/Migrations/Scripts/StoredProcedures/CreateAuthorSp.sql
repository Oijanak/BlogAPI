CREATE OR ALTER PROCEDURE spCreateAuthor
    @AuthorEmail NVARCHAR(255),
    @AuthorName NVARCHAR(255),
    @Age INT,
    @CreatedBy NVARCHAR(255)
    AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AuthorId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Authors (AuthorId, Age, AuthorEmail, AuthorName,CreatedBy)
    OUTPUT inserted.*
VALUES (@AuthorId, @Age, @AuthorEmail, @AuthorName,@CreatedBy);
END;