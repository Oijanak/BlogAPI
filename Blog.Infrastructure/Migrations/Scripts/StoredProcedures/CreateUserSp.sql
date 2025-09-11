CREATE OR ALTER PROCEDURE spCreateUser

    @Name NVARCHAR(MAX),
    @Email NVARCHAR(MAX),
    @PasswordHash NVARCHAR(MAX)
    AS
BEGIN
        SET NOCOUNT ON;

INSERT INTO [Users] (Name, Email, PasswordHash)
    OUTPUT inserted.*
VALUES (@Name, @Email, @PasswordHash);
END;
