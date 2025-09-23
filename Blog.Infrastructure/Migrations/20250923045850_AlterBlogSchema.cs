using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterBlogSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveStatus",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApproveStatus",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserId",
                table: "Blogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AspNetUsers_ApprovedByUserId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_ApprovedByUserId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ActiveStatus",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ApproveStatus",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Blogs");
        }
    }
}
