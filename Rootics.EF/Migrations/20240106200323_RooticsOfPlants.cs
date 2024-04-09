using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rootics.EF.Migrations
{
    /// <inheritdoc />
    public partial class RooticsOfPlants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Species = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DatePlanted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HeightInCentimeters = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WidthInCentimeters = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlantImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plants_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CareAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    NextWateringDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LastWateringDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WateringFrequencyDays = table.Column<int>(type: "int", nullable: false),
                    FertilizingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FertilizingNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareAlerts_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantDiseases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Symptoms = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cause = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Prevention = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Treatment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DiscoveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantDiseases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantDiseases_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlantDiseaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatments_PlantDiseases_PlantDiseaseId",
                        column: x => x.PlantDiseaseId,
                        principalTable: "PlantDiseases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareAlerts_PlantId",
                table: "CareAlerts",
                column: "PlantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlantDiseases_PlantId",
                table: "PlantDiseases",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_ApplicationUserId",
                table: "Plants",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PlantDiseaseId",
                table: "Treatments",
                column: "PlantDiseaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareAlerts");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "PlantDiseases");

            migrationBuilder.DropTable(
                name: "Plants");
        }
    }
}
