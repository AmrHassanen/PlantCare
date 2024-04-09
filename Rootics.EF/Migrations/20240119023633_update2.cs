using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rootics.EF.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeightInCentimeters",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "WidthInCentimeters",
                table: "Plants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HeightInCentimeters",
                table: "Plants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WidthInCentimeters",
                table: "Plants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
