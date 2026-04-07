using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleMaintenance.Migrations
{
    /// <inheritdoc />
    public partial class maintenancerecordcomponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRecords_VehicleComponents_ComponentId",
                table: "MaintenanceRecords");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceRecords_ComponentId",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "ComponentId",
                table: "MaintenanceRecords");

            migrationBuilder.CreateTable(
                name: "MaintenanceRecordComponents",
                columns: table => new
                {
                    MaintenanceRecordComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceRecordId = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    ChangeType = table.Column<int>(type: "int", nullable: false),
                    WorkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedParts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldState = table.Column<int>(type: "int", nullable: true),
                    NewState = table.Column<int>(type: "int", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LaborMinutes = table.Column<int>(type: "int", nullable: true),
                    LaborCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PartsCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TechnicianName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorOrShop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecordComponents", x => x.MaintenanceRecordComponentId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecordComponents_MaintenanceRecords_MaintenanceRecordId",
                        column: x => x.MaintenanceRecordId,
                        principalTable: "MaintenanceRecords",
                        principalColumn: "MaintenanceRecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecordComponents_VehicleComponents_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "VehicleComponents",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecordComponents_ComponentId",
                table: "MaintenanceRecordComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecordComponents_MaintenanceRecordId",
                table: "MaintenanceRecordComponents",
                column: "MaintenanceRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceRecordComponents");

            migrationBuilder.AddColumn<int>(
                name: "ComponentId",
                table: "MaintenanceRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_ComponentId",
                table: "MaintenanceRecords",
                column: "ComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRecords_VehicleComponents_ComponentId",
                table: "MaintenanceRecords",
                column: "ComponentId",
                principalTable: "VehicleComponents",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
