CREATE OR ALTER PROCEDURE spUpdateUser
    @UserId UNIQUEIDENTIFIER,
    @Name NVARCHAR(MAX),
    @Email NVARCHAR(MAX),
    @PasswordHash NVARCHAR(MAX)
    AS
BEGIN
    SET NOCOUNT ON;

UPDATE [Users]
SET
    Name = @Name,
    Email = @Email,
    PasswordHash = @PasswordHash
    OUTPUT inserted.*
WHERE UserId = @UserId;
END;