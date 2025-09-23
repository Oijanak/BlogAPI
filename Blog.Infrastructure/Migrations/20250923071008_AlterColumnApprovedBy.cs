using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnApprovedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_ApprovedByUserId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_ApprovedByUserId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ApprovedBy",
                table: "Blogs",
                column: "ApprovedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_ApprovedBy",
                table: "Blogs",
                column: "ApprovedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_ApprovedBy",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_ApprovedBy",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserId",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ApprovedByUserId",
                table: "Blogs",
                column: "ApprovedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AspNetUsers_ApprovedByUserId",
                table: "Blogs",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
