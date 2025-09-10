using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpCreateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
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
    END
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS spCreateUser");      
        }
    }
}
