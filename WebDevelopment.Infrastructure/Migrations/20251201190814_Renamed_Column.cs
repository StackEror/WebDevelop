using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDevelopment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Renamed_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsIsand",
                table: "Countries",
                newName: "IsIsland");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsIsland",
                table: "Countries",
                newName: "IsIsand");
        }
    }
}
