using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rootics.EF.Migrations
{
    /// <inheritdoc />
    public partial class update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_PlantDiseases_PlantDiseaseId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Treatments");

            migrationBuilder.AlterColumn<int>(
                name: "PlantDiseaseId",
                table: "Treatments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_PlantDiseases_PlantDiseaseId",
                table: "Treatments",
                column: "PlantDiseaseId",
                principalTable: "PlantDiseases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_PlantDiseases_PlantDiseaseId",
                table: "Treatments");

            migrationBuilder.AlterColumn<int>(
                name: "PlantDiseaseId",
                table: "Treatments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Treatments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Treatments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_PlantDiseases_PlantDiseaseId",
                table: "Treatments",
                column: "PlantDiseaseId",
                principalTable: "PlantDiseases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
