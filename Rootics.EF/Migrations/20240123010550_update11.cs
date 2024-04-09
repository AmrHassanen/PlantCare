using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rootics.EF.Migrations
{
    /// <inheritdoc />
    public partial class update11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plants_CareAlertOfPlantId",
                table: "Plants");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_CareAlertOfPlantId",
                table: "Plants",
                column: "CareAlertOfPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_CareAlerts_PlantId",
                table: "CareAlerts",
                column: "PlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareAlerts_Plants_PlantId",
                table: "CareAlerts",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareAlerts_Plants_PlantId",
                table: "CareAlerts");

            migrationBuilder.DropIndex(
                name: "IX_Plants_CareAlertOfPlantId",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_CareAlerts_PlantId",
                table: "CareAlerts");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_CareAlertOfPlantId",
                table: "Plants",
                column: "CareAlertOfPlantId",
                unique: true);
        }
    }
}
