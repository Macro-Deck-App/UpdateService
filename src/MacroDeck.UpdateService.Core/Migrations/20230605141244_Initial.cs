using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MacroDeck.UpdateService.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "updateservice");

            migrationBuilder.CreateTable(
                name: "versions",
                schema: "updateservice",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    version_string = table.Column<string>(type: "text", nullable: false),
                    version_major = table.Column<int>(type: "integer", nullable: false),
                    version_minor = table.Column<int>(type: "integer", nullable: false),
                    version_patch = table.Column<int>(type: "integer", nullable: false),
                    version_pre_release_no = table.Column<int>(type: "integer", nullable: true),
                    is_pre_release_version = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "version_files",
                schema: "updateservice",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_provider = table.Column<int>(type: "integer", nullable: false),
                    platform_identifier = table.Column<int>(type: "integer", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    version_ref = table.Column<int>(type: "integer", nullable: false),
                    created_timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_version_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_version_files_versions_version_ref",
                        column: x => x.version_ref,
                        principalSchema: "updateservice",
                        principalTable: "versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_version_files_version_ref",
                schema: "updateservice",
                table: "version_files",
                column: "version_ref");

            migrationBuilder.CreateIndex(
                name: "IX_versions_version_string",
                schema: "updateservice",
                table: "versions",
                column: "version_string",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "version_files",
                schema: "updateservice");

            migrationBuilder.DropTable(
                name: "versions",
                schema: "updateservice");
        }
    }
}
