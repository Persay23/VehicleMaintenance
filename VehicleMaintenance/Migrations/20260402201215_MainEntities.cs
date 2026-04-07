using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class MainEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiquidEntries",
                columns: table => new
                {
                    LiquidEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    LiquidType = table.Column<int>(type: "int", nullable: false),
                    RefillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidEntries", x => x.LiquidEntryId);
                    table.ForeignKey(
                        name: "FK_LiquidEntries_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    PredictionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    ComponentType = table.Column<int>(type: "int", nullable: false),
                    PredictedServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfidenceScore = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleComponentComponentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.PredictionId);
                    table.ForeignKey(
                        name: "FK_Predictions_VehicleComponents_VehicleComponentComponentId",
                        column: x => x.VehicleComponentComponentId,
                        principalTable: "VehicleComponents",
                        principalColumn: "ComponentId");
                    table.ForeignKey(
                        name: "FK_Predictions_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    MaintenanceRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<int>(type: "int", nullable: true),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PredictionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.MaintenanceRecordId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Predictions_PredictionId",
                        column: x => x.PredictionId,
                        principalTable: "Predictions",
                        principalColumn: "PredictionId");
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_VehicleComponents_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "VehicleComponents",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidEntries_VehicleId",
                table: "LiquidEntries",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_ComponentId",
                table: "MaintenanceRecords",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_PredictionId",
                table: "MaintenanceRecords",
                column: "PredictionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_VehicleId",
                table: "MaintenanceRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_VehicleComponentComponentId",
                table: "Predictions",
                column: "VehicleComponentComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_VehicleId",
                table: "Predictions",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiquidEntries");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "Predictions");
        }
    }
}
