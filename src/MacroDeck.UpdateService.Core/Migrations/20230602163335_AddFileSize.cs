using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacroDeck.UpdateService.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddFileSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "file_size",
                schema: "updateservice",
                table: "version_files",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_size",
                schema: "updateservice",
                table: "version_files");
        }
    }
}
