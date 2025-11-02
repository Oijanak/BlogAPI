using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterGetALLBlogsp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames =
                assembly.GetManifestResourceNames().
                    Where(str => str.EndsWith(".sql"));
            foreach (string resourceName in resourceNames)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string sql = reader.ReadToEnd();
                    migrationBuilder.Sql(sql);
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "BlogApi.Infrastructure.Data.Migrations.Scripts.DropProcedures.sql";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            var sql = reader.ReadToEnd();
            migrationBuilder.Sql(sql);
        }
    }
}
