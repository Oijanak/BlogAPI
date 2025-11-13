using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ActiveStatus_ApproveStatus",
                table: "Blogs",
                columns: new[] { "ActiveStatus", "ApproveStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CreatedAt",
                table: "Blogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_StartDate_EndDate",
                table: "Blogs",
                columns: new[] { "StartDate", "EndDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_ActiveStatus_ApproveStatus",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_CreatedAt",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_StartDate_EndDate",
                table: "Blogs");
        }
    }
}
