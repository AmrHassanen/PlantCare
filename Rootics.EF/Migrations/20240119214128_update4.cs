using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rootics.EF.Migrations
{
    /// <inheritdoc />
    public partial class update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Treatment",
                table: "PlantDiseases");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Treatment",
                table: "PlantDiseases",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
