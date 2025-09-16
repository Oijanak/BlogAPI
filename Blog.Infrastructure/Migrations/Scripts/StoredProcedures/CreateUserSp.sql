CREATE OR ALTER PROCEDURE spCreateUser

    @Name NVARCHAR(MAX),
    @Email NVARCHAR(MAX),
    @PasswordHash NVARCHAR(MAX)
    AS
BEGIN
        SET NOCOUNT ON;
        DECLARE @NewUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [Users] (UserId,Name, Email, PasswordHash)
    OUTPUT inserted.*
VALUES (@NewUserId,@Name, @Email, @PasswordHash);
END;
