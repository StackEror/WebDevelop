using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDevelopment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "AspNetUsers",
                newName: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "AspNetUsers",
                newName: "Role");
        }
    }
}
