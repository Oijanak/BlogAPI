using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spFolder = Path.Combine(AppContext.BaseDirectory, "Migrations", "Scripts", "StoredProcedures");
            if (Directory.Exists(spFolder))
            {
                foreach (var file in Directory.GetFiles(spFolder, "*.sql"))
                {
                    var sql = File.ReadAllText(file);
                    migrationBuilder.Sql(sql);
                }
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropSqlPath = Path.Combine(AppContext.BaseDirectory, "Migrations", "Scripts", "DropProcedures.sql");

            if (File.Exists(dropSqlPath))
            {
                var sqlContent = File.ReadAllText(dropSqlPath);
                if (!string.IsNullOrWhiteSpace(sqlContent))
                {
                    migrationBuilder.Sql(sqlContent);
                }
            }
        }
    }
}