using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAndUpdatatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Authors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Authors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CreatedBy",
                table: "Blogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UpdatedBy",
                table: "Blogs",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CreatedBy",
                table: "Authors",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_UpdatedBy",
                table: "Authors",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AspNetUsers_CreatedBy",
                table: "Authors",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AspNetUsers_UpdatedBy",
                table: "Authors",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_CreatedBy",
                table: "Blogs",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_UpdatedBy",
                table: "Blogs",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AspNetUsers_CreatedBy",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AspNetUsers_UpdatedBy",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_CreatedBy",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_UpdatedBy",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_CreatedBy",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_UpdatedBy",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CreatedBy",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_UpdatedBy",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Authors");
        }
    }
}
