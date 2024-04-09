using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rootics.EF.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantDiseases_Plants_PlantId",
                table: "PlantDiseases");

            migrationBuilder.AlterColumn<int>(
                name: "PlantId",
                table: "PlantDiseases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantDiseases_Plants_PlantId",
                table: "PlantDiseases",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantDiseases_Plants_PlantId",
                table: "PlantDiseases");

            migrationBuilder.AlterColumn<int>(
                name: "PlantId",
                table: "PlantDiseases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantDiseases_Plants_PlantId",
                table: "PlantDiseases",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
